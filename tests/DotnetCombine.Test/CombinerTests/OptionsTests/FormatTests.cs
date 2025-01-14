﻿using DotnetCombine.Options;
using DotnetCombine.Services;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DotnetCombine.Test.CombinerTests.OptionsTests
{

    public class FormatTests : BaseCombinerTests
    {
        [Fact]
        public async Task Format_Indents()
        {
            // Arrange - create a pre-existing 'output' file
            var initialCsFile = Path.Combine(DefaultOutputDir, nameof(Format_Indents)) + OutputFileBase.CSharpOutputExtension;

            // Act
            var options = new CombineOptions()
            {
                OverWrite = true,
                Input = InputDir,
                Output = initialCsFile,
                Format = true
            };

            var exitCode = await new Combiner(options).Run();

            // Assert - final file isn't initial file
            Assert.Equal(0, exitCode);
            Assert.True(File.Exists(initialCsFile));
            var allLines = File.ReadAllLines(initialCsFile);
            Assert.DoesNotContain(allLines, line => line.StartsWith("public"));
            Assert.DoesNotContain(allLines, line => line.StartsWith("internal"));
            Assert.StartsWith("// File generated by dotnet-combine", allLines[0]);
        }

        [Fact]
        public async Task NoFormat_NoIndents()
        {
            // Arrange - create a pre-existing 'output' file
            var initialCsFile = Path.Combine(DefaultOutputDir, nameof(Format_Indents)) + OutputFileBase.CSharpOutputExtension;

            // Act
            var options = new CombineOptions()
            {
                OverWrite = true,
                Input = InputDir,
                Output = initialCsFile,
                Format = false
            };

            var exitCode = await new Combiner(options).Run();

            // Assert - final file isn't initial file
            Assert.Equal(0, exitCode);
            Assert.True(File.Exists(initialCsFile));
            var allLines = File.ReadAllLines(initialCsFile);
            Assert.Contains(allLines, line => line.StartsWith("public"));
            Assert.Contains(allLines, line => line.StartsWith("internal"));
            Assert.StartsWith("// File generated by dotnet-combine", allLines[0]);
        }
    }
}