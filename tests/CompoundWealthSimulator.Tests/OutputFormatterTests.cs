using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;
using CompoundWealthSimulator;

namespace CompoundWealthSimulator
{
    public class OutputFormatterTests
    {
        private readonly OutputFormatter _formatter = new OutputFormatter();

        [Fact]
        public void FormatTable_ThrowsArgumentNullException_WhenRowsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _formatter.FormatTable(null!));
        }

        [Fact]
        public void FormatTable_ReturnsOnlyHeaderAndSeparator_WhenRowsIsEmpty()
        {
            var rows = new List<ProjectionRow>();
            var expected = "Year | Total Contributions | Cumulative Interest |          End Balance\n" +
                           "-----+---------------------+---------------------+---------------------\n";

            var result = _formatter.FormatTable(rows);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void FormatTable_FormatsSingleRowCorrectly_AcceptanceExample()
        {
            var rows = new List<ProjectionRow>
            {
                new ProjectionRow
                {
                    Year = 1,
                    TotalContributions = 1200.00m,
                    CumulativeInterest = 50.35m,
                    EndBalance = 1250.35m
                }
            };

            var expected = "Year | Total Contributions | Cumulative Interest |          End Balance\n" +
                           "-----+---------------------+---------------------+---------------------\n" +
                           "   1 |           $1,200.00 |              $50.35 |           $1,250.35\n";

            var result = _formatter.FormatTable(rows);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void FormatTable_FormatsMultipleRowsCorrectly(List<ProjectionRow> rows, string expectedPart)
        {
            var result = _formatter.FormatTable(rows);
            Assert.Contains(expectedPart, result);
        }

        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[]
            {
                new List<ProjectionRow>
                {
                    new ProjectionRow { Year = 1, TotalContributions = 1000m, CumulativeInterest = 50m, EndBalance = 1050m },
                    new ProjectionRow { Year = 2, TotalContributions = 2000m, CumulativeInterest = 150m, EndBalance = 2150m }
                },
                "   1 |           $1,000.00 |              $50.00 |           $1,050.00\n" +
                "   2 |           $2,000.00 |             $150.00 |           $2,150.00\n"
            };

            yield return new object[]
            {
                new List<ProjectionRow>
                {
                    new ProjectionRow { Year = 0, TotalContributions = 0m, CumulativeInterest = 0m, EndBalance = 0m }
                },
                "   0 |               $0.00 |               $0.00 |               $0.00\n"
            };

            yield return new object[]
            {
                new List<ProjectionRow>
                {
                    new ProjectionRow { Year = 99, TotalContributions = 1000000.00m, CumulativeInterest = 9999999.99m, EndBalance = 10999999.99m }
                },
                "  99 |       $1,000,000.00 |       $9,999,999.99 |      $10,999,999.99\n"
            };
        }

        [Fact]
        public void FormatTable_IsCultureIndependent()
        {
            var originalCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("fr-FR");

                var rows = new List<ProjectionRow>
                {
                    new ProjectionRow
                    {
                        Year = 1,
                        TotalContributions = 1200.00m,
                        CumulativeInterest = 50.35m,
                        EndBalance = 1250.35m
                    }
                };

                var expected = "Year | Total Contributions | Cumulative Interest |          End Balance\n" +
                               "-----+---------------------+---------------------+---------------------\n" +
                               "   1 |           $1,200.00 |              $50.35 |           $1,250.35\n";

                var result = _formatter.FormatTable(rows);

                Assert.Equal(expected, result);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }
    }
}
