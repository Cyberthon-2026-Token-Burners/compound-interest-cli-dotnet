using System;
using System.Collections.Generic;
using Xunit;

namespace CompoundWealthSimulator
{
    public class SimulatorTests
    {
        private readonly Simulator _simulator = new Simulator();

        [Fact]
        public void Project_NullParameters_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _simulator.Project(null!));
        }

        [Theory]
        [InlineData((Frequency)0)]
        [InlineData((Frequency)99)]
        [InlineData((Frequency)(-1))]
        public void Project_InvalidFrequency_ThrowsArgumentException(Frequency frequency)
        {
            var parameters = new SimulationParameters(
                Principal: 1000m,
                Contribution: 100m,
                ContributionFrequency: frequency,
                AnnualInterestRate: 0.05m,
                DurationYears: 5
            );
            Assert.Throws<ArgumentException>(() => _simulator.Project(parameters));
        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(-100.0)]
        [InlineData(100000000.01)]
        public void Project_PrincipalOutOfBounds_ThrowsArgumentException(decimal principal)
        {
            var parameters = new SimulationParameters(
                Principal: principal,
                Contribution: 100m,
                ContributionFrequency: Frequency.Monthly,
                AnnualInterestRate: 0.05m,
                DurationYears: 5
            );
            Assert.Throws<ArgumentException>(() => _simulator.Project(parameters));
        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(-50.0)]
        [InlineData(10000000.01)]
        public void Project_ContributionOutOfBounds_ThrowsArgumentException(decimal contribution)
        {
            var parameters = new SimulationParameters(
                Principal: 1000m,
                Contribution: contribution,
                ContributionFrequency: Frequency.Monthly,
                AnnualInterestRate: 0.05m,
                DurationYears: 5
            );
            Assert.Throws<ArgumentException>(() => _simulator.Project(parameters));
        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(-1.5)]
        [InlineData(2.01)]
        [InlineData(5.0)]
        public void Project_AnnualInterestRateOutOfBounds_ThrowsArgumentException(decimal rate)
        {
            var parameters = new SimulationParameters(
                Principal: 1000m,
                Contribution: 100m,
                ContributionFrequency: Frequency.Monthly,
                AnnualInterestRate: rate,
                DurationYears: 5
            );
            Assert.Throws<ArgumentException>(() => _simulator.Project(parameters));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(101)]
        [InlineData(200)]
        public void Project_DurationYearsOutOfBounds_ThrowsArgumentException(int duration)
        {
            var parameters = new SimulationParameters(
                Principal: 1000m,
                Contribution: 100m,
                ContributionFrequency: Frequency.Monthly,
                AnnualInterestRate: 0.05m,
                DurationYears: duration
            );
            Assert.Throws<ArgumentException>(() => _simulator.Project(parameters));
        }

        [Fact]
        public void Project_ValidationPrecedence_PrincipalBeforeContribution()
        {
            var parameters = new SimulationParameters(
                Principal: -1m,
                Contribution: -1m,
                ContributionFrequency: Frequency.Monthly,
                AnnualInterestRate: 0.05m,
                DurationYears: 5
            );
            Assert.Throws<ArgumentException>(() => _simulator.Project(parameters));
        }

        [Fact]
        public void Project_ValidationPrecedence_ContributionBeforeInterest()
        {
            var parameters = new SimulationParameters(
                Principal: 1000m,
                Contribution: -1m,
                ContributionFrequency: Frequency.Monthly,
                AnnualInterestRate: -0.05m,
                DurationYears: 5
            );
            Assert.Throws<ArgumentException>(() => _simulator.Project(parameters));
        }

        [Fact]
        public void Project_ValidationPrecedence_InterestBeforeDuration()
        {
            var parameters = new SimulationParameters(
                Principal: 1000m,
                Contribution: 100m,
                ContributionFrequency: Frequency.Monthly,
                AnnualInterestRate: -0.05m,
                DurationYears: -5
            );
            Assert.Throws<ArgumentException>(() => _simulator.Project(parameters));
        }

        [Fact]
        public void Project_ZeroDurationYears_ReturnsEmptyList()
        {
            var parameters = new SimulationParameters(
                Principal: 1000m,
                Contribution: 100m,
                ContributionFrequency: Frequency.Monthly,
                AnnualInterestRate: 0.05m,
                DurationYears: 0
            );

            var result = _simulator.Project(parameters);
            Assert.Empty(result);
        }

        [Fact]
        public void Project_AcceptanceExample1()
        {
            var parameters = new SimulationParameters(
                Principal: 1000m,
                Contribution: 100m,
                ContributionFrequency: Frequency.Monthly,
                AnnualInterestRate: 0m,
                DurationYears: 1
            );

            var result = _simulator.Project(parameters);

            Assert.Single(result);
            var row = result[0];
            Assert.Equal(1, row.Year);
            Assert.Equal(1200m, row.TotalContributions);
            Assert.Equal(0m, row.CumulativeInterest);
            Assert.Equal(2200m, row.EndBalance);
        }

        [Fact]
        public void Project_AcceptanceExample2()
        {
            var parameters = new SimulationParameters(
                Principal: 1000m,
                Contribution: 0m,
                ContributionFrequency: Frequency.Annually,
                AnnualInterestRate: 0.10m,
                DurationYears: 2
            );

            var result = _simulator.Project(parameters);

            Assert.Equal(2, result.Count);

            var row1 = result[0];
            Assert.Equal(1, row1.Year);
            Assert.Equal(0m, row1.TotalContributions);
            Assert.Equal(100m, row1.CumulativeInterest);
            Assert.Equal(1100m, row1.EndBalance);

            var row2 = result[1];
            Assert.Equal(2, row2.Year);
            Assert.Equal(0m, row2.TotalContributions);
            Assert.Equal(210m, row2.CumulativeInterest);
            Assert.Equal(1210m, row2.EndBalance);
        }

        [Fact]
        public void Project_BoundaryValues_MinimumValidValues()
        {
            var parameters = new SimulationParameters(
                Principal: 0m,
                Contribution: 0m,
                ContributionFrequency: Frequency.Annually,
                AnnualInterestRate: 0m,
                DurationYears: 1
            );

            var result = _simulator.Project(parameters);
            Assert.Single(result);
            Assert.Equal(0m, result[0].TotalContributions);
            Assert.Equal(0m, result[0].CumulativeInterest);
            Assert.Equal(0m, result[0].EndBalance);
        }

        [Fact]
        public void Project_BoundaryValues_MaximumValidValues()
        {
            var parameters = new SimulationParameters(
                Principal: 100_000_000m,
                Contribution: 10_000_000m,
                ContributionFrequency: Frequency.Annually,
                AnnualInterestRate: 2m,
                DurationYears: 1
            );

            var result = _simulator.Project(parameters);
            Assert.Single(result);
            Assert.Equal(10_000_000m, result[0].TotalContributions);
            Assert.Equal(200_000_000m, result[0].CumulativeInterest);
            Assert.Equal(310_000_000m, result[0].EndBalance);
        }

        [Theory]
        [InlineData(Frequency.Monthly, 12)]
        [InlineData(Frequency.Quarterly, 4)]
        [InlineData(Frequency.Annually, 1)]
        public void Project_DifferentFrequencies(Frequency frequency, int expectedPeriods)
        {
            var parameters = new SimulationParameters(
                Principal: 1000m,
                Contribution: 10m,
                ContributionFrequency: frequency,
                AnnualInterestRate: 0.12m,
                DurationYears: 1
            );

            var result = _simulator.Project(parameters);
            Assert.Single(result);

            decimal balance = 1000m;
            decimal totalContrib = 0m;
            decimal periodicRate = 0.12m / expectedPeriods;
            decimal cumulativeInterest = 0m;

            for (int i = 0; i < expectedPeriods; i++)
            {
                decimal interest = balance * periodicRate;
                balance += 10m;
                totalContrib += 10m;
                balance += interest;
                cumulativeInterest += interest;
            }

            Assert.Equal(1, result[0].Year);
            Assert.Equal(totalContrib, result[0].TotalContributions);
            Assert.Equal(cumulativeInterest, result[0].CumulativeInterest);
            Assert.Equal(balance, result[0].EndBalance);
        }
    }
}
