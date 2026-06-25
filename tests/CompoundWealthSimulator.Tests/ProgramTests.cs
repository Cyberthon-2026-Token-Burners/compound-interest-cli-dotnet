using System;
using System.IO;
using Xunit;

namespace CompoundWealthSimulator
{
    public class ProgramTests
    {
        [Fact]
        public void Main_WithNoArguments_ReturnsZeroAndPrintsInitializationMessage()
        {
            var originalOut = Console.Out;
            using var sw = new StringWriter();
            Console.SetOut(sw);
            try
            {
                int exitCode = Program.Main(Array.Empty<string>());
                Assert.Equal(0, exitCode);
                string output = sw.ToString().Trim();
                Assert.Equal("Compound Wealth Simulator Initialized.", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Theory]
        [InlineData(new object[] { new string[] { "--help" } })]
        [InlineData(new object[] { new string[] { "-v", "detailed" } })]
        [InlineData(new object[] { new string[] { "", " " } })]
        public void Main_WithVariousArguments_ReturnsZeroAndPrintsInitializationMessage(string[] args)
        {
            var originalOut = Console.Out;
            using var sw = new StringWriter();
            Console.SetOut(sw);
            try
            {
                int exitCode = Program.Main(args);
                Assert.Equal(0, exitCode);
                string output = sw.ToString().Trim();
                Assert.Equal("Compound Wealth Simulator Initialized.", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }
}
