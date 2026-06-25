using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CompoundWealthSimulator
{
    public interface IOutputFormatter
    {
        string FormatTable(List<ProjectionRow> rows);
    }

    public class OutputFormatter : IOutputFormatter
    {
        private const string Header = "Year | Total Contributions | Cumulative Interest |          End Balance";
        private const string Separator = "-----+---------------------+---------------------+---------------------";

        public string FormatTable(List<ProjectionRow> rows)
        {
            if (rows is null)
                throw new ArgumentNullException(nameof(rows));

            var sb = new StringBuilder();
            sb.Append(Header).Append('\n');
            sb.Append(Separator).Append('\n');

            foreach (var row in rows)
            {
                sb.Append(FormatRow(row)).Append('\n');
            }

            return sb.ToString();
        }

        private static string FormatCurrency(decimal value)
            => "$" + value.ToString("N2", CultureInfo.InvariantCulture);

        private static string FormatRow(ProjectionRow row)
        {
            var year = row.Year.ToString(CultureInfo.InvariantCulture).PadLeft(4);
            var contributions = FormatCurrency(row.TotalContributions).PadLeft(19);
            var interest = FormatCurrency(row.CumulativeInterest).PadLeft(19);
            var balance = FormatCurrency(row.EndBalance).PadLeft(19);
            return $"{year} | {contributions} | {interest} | {balance}";
        }
    }
}
