using System;
using System.Collections.Generic;

namespace CompoundWealthSimulator
{
    public enum Frequency
    {
        Monthly = 12,
        Quarterly = 4,
        Annually = 1
    }

    public record SimulationParameters(
        decimal Principal,
        decimal Contribution,
        Frequency ContributionFrequency,
        decimal AnnualInterestRate,
        int DurationYears
    );

    public struct ProjectionRow
    {
        public int Year { get; init; }
        public decimal TotalContributions { get; init; }
        public decimal CumulativeInterest { get; init; }
        public decimal EndBalance { get; init; }
    }

    public interface ISimulator
    {
        List<ProjectionRow> Project(SimulationParameters parameters);
    }

    public class Simulator : ISimulator
    {
        public List<ProjectionRow> Project(SimulationParameters parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);

            if (!Enum.IsDefined(typeof(Frequency), parameters.ContributionFrequency))
                throw new ArgumentException(
                    "ContributionFrequency must be a valid Frequency enum value.",
                    nameof(parameters));

            if (parameters.Principal < 0m || parameters.Principal > 100_000_000m)
                throw new ArgumentException(
                    "Principal must be between 0.00 and 100,000,000.00.",
                    nameof(parameters));

            if (parameters.Contribution < 0m || parameters.Contribution > 10_000_000m)
                throw new ArgumentException(
                    "Contribution must be between 0.00 and 10,000,000.00.",
                    nameof(parameters));

            if (parameters.AnnualInterestRate < 0m || parameters.AnnualInterestRate > 2m)
                throw new ArgumentException(
                    "AnnualInterestRate must be between 0.00 and 2.00.",
                    nameof(parameters));

            if (parameters.DurationYears < 0 || parameters.DurationYears > 100)
                throw new ArgumentException(
                    "DurationYears must be between 0 and 100.",
                    nameof(parameters));

            if (parameters.DurationYears == 0)
                return new List<ProjectionRow>();

            int n = (int)parameters.ContributionFrequency;
            int totalPeriods = parameters.DurationYears * n;

            decimal currentBalance = parameters.Principal;
            decimal totalContributions = 0m;
            decimal cumulativeInterest = 0m;

            var rows = new List<ProjectionRow>(parameters.DurationYears);

            for (int period = 1; period <= totalPeriods; period++)
            {
                decimal interest = currentBalance * (parameters.AnnualInterestRate / (decimal)n);
                currentBalance += parameters.Contribution;
                totalContributions += parameters.Contribution;
                currentBalance += interest;
                cumulativeInterest += interest;

                if (period % n == 0)
                {
                    rows.Add(new ProjectionRow
                    {
                        Year = period / n,
                        TotalContributions = totalContributions,
                        CumulativeInterest = cumulativeInterest,
                        EndBalance = currentBalance
                    });
                }
            }

            return rows;
        }
    }
}
