﻿using DotnetCombine.Options;
using DotnetCombine.Services;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DotnetCombine.Test.CombinerTests.OptionsTests
{

    public class OutputTests : BaseCombinerTests
    {
        [Theory]
        [InlineData("OutPutfilename")]
        [InlineData("OutPutfilename.cpp")]
        [InlineData("OutPutfilename" + OutputFileBase.CSharpOutputExtension)]
        [InlineData(DefaultOutputDir + "/OutPutfilename")]
        [InlineData(DefaultOutputDir + "/nonexistingFolder/OutPutfilename")]
        [InlineData(DefaultOutputDir + "/nonexistingFolder/OutPutfilename" + OutputFileBase.CSharpOutputExtension)]
        public async Task OutPut(string output)
        {
            // Act
            var options = new CombineOptions()
            {
                Output = output,
                OverWrite = true,
                Input = InputDir,
                Suffix = CombinerTestsFixture.DefaultSuffix
            };

            var exitCode = await new Combiner(options).Run();

            // Assert
            Assert.Equal(0, exitCode);
            var path = string.IsNullOrEmpty(Path.GetDirectoryName(output))
                ? InputDir
                : Path.GetDirectoryName(output)!;
            var existingFiles = Directory.GetFiles(path);

            var zipFiles = existingFiles.Where(f => Path.GetFileNameWithoutExtension(f) == $"OutPutfilename{options.Suffix}" && Path.GetExtension(f) == OutputFileBase.CSharpOutputExtension);

            Assert.Single(zipFiles);
        }

        [Theory]
        [InlineData(DefaultOutputDir + "/")]
        [InlineData(DefaultOutputDir + "//")]
        public async Task NoOutputFileName_GeneratesUniqueFileName(string outputDir)
        {
            // Arrange
            string prefix = $"{nameof(NoOutputFileName_GeneratesUniqueFileName)}-{Guid.NewGuid()}-";
            var timeBefore = ParseDateTimeFromFileName(UniqueIdGenerator.UniqueId());

            // Act
            var options = new CombineOptions()
            {
                Output = outputDir,
                OverWrite = true,
                Input = InputDir,
                Prefix = prefix,
                Suffix = CombinerTestsFixture.DefaultSuffix
            };

            var exitCode = await new Combiner(options).Run();

            // Assert
            Assert.Equal(0, exitCode);
            var existingFiles = Directory.GetFiles(outputDir);

            var timeAfter = ParseDateTimeFromFileName(UniqueIdGenerator.UniqueId());
            var csGeneratedFiles = existingFiles.Where(f =>
            {
                var fileName = Path.GetFileNameWithoutExtension(f);
                if (fileName.StartsWith(prefix) && Path.GetExtension(f) == OutputFileBase.CSharpOutputExtension)
                {
                    var trimmedFileName = fileName[prefix.Length..^options.Suffix.Length];
                    var fileDate = ParseDateTimeFromFileName(trimmedFileName);
                    return fileDate is not null && fileDate >= timeBefore && fileDate <= timeAfter;
                }

                return false;
            });

            Assert.Single(csGeneratedFiles);
        }

        [Theory]
        [InlineData(nameof(NoOutputDir_UsesInputDirAndOutputFileName), "")]
        [InlineData(nameof(NoOutputDir_UsesInputDirAndOutputFileName), OutputFileBase.CSharpOutputExtension)]
        public async Task NoOutputDir_UsesInputDirAndOutputFileName(string fileName, string extension)
        {
            // Act
            var options = new CombineOptions()
            {
                Output = fileName + extension,
                OverWrite = true,
                Input = InputDir,
                Suffix = CombinerTestsFixture.DefaultSuffix
            };

            var exitCode = await new Combiner(options).Run();

            // Assert
            Assert.Equal(0, exitCode);
            var existingFiles = Directory.GetFiles(InputDir);

            var csGeneratedFiles = existingFiles.Where(f => Path.GetFileNameWithoutExtension(f) == fileName + options.Suffix && Path.GetExtension(f) == OutputFileBase.CSharpOutputExtension);

            Assert.Single(csGeneratedFiles);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", OutputFileBase.CSharpOutputExtension)]
        public async Task NoOutputDir_UsesInputDirAndOutputFileNameWithPrefixWithParentFolder(string fileName, string extension)
        {
            // Act
            var options = new CombineOptions()
            {
                Output = fileName + extension,
                OverWrite = true,
                Input = InputDir,
                Suffix = CombinerTestsFixture.DefaultSuffix,
                PrefixWithParentFolder = true
            };

            var obj = new Combiner(options);
            
            var exitCode = await obj.Run();

            // Assert
            Assert.Equal(0, exitCode);
            var existingFiles = Directory.GetFiles(InputDir);
            var csGeneratedFiles = existingFiles.Where(f => Path.GetFileName(f) == obj.fileName && Path.GetExtension(f) == OutputFileBase.CSharpOutputExtension);
            Assert.Single(csGeneratedFiles);
        }

        [Fact]
        public async Task NoOutput_UsesInputDirAndGeneratesUniqueFileName()
        {
            // Arrange
            var timeBefore = ParseDateTimeFromFileName(UniqueIdGenerator.UniqueId());

            // Act
            var options = new CombineOptions()
            {
                Output = null,
                Input = InputDir,
                OverWrite = true,
                Suffix = CombinerTestsFixture.DefaultSuffix
            };

            var exitCode = await new Combiner(options).Run();

            // Assert
            Assert.Equal(0, exitCode);
            var existingFiles = Directory.GetFiles(InputDir);

            var timeAfter = ParseDateTimeFromFileName(UniqueIdGenerator.UniqueId());
            var csGeneratedFiles = existingFiles.Where(f =>
            {
                if (Path.GetExtension(f) == OutputFileBase.CSharpOutputExtension)
                {
                    var fileDate = ParseDateTimeFromFileName(Path.GetFileNameWithoutExtension(f).Replace(options.Suffix, string.Empty));
                    return fileDate is not null && fileDate >= timeBefore && fileDate <= timeAfter;
                }

                return false;
            });

            Assert.Single(csGeneratedFiles);
        }

        [Theory]
        [InlineData(DefaultOutputDir + "/OutputPrefix/")]
        [InlineData(DefaultOutputDir + "/OutputPrefix/filename")]
        [InlineData(DefaultOutputDir + "/OutputPrefix/filename" + OutputFileBase.CSharpOutputExtension)]
        public async Task OutputPrefix(string output)
        {
            // Arrange
            var prefix = $"prefix-{output.Replace("/", "-")}-";

            // Act
            var options = new CombineOptions()
            {
                Prefix = prefix,
                Output = output,
                Input = InputDir
            };

            var exitCode = await new Combiner(options).Run();

            // Assert
            Assert.Equal(0, exitCode);
            var path = string.IsNullOrEmpty(Path.GetDirectoryName(output))
                ? InputDir
                : Path.GetDirectoryName(output)!;
            var existingFiles = Directory.GetFiles(path);

            var csGeneratedFiles = existingFiles.Where(f =>
                Path.GetFileNameWithoutExtension(f).StartsWith(prefix)
                && Path.GetExtension(f) == OutputFileBase.CSharpOutputExtension);

            Assert.Single(csGeneratedFiles);
        }

        [Theory]
        [InlineData(DefaultOutputDir + "/OutputSuffix/")]
        [InlineData(DefaultOutputDir + "/OutputSuffix/filename")]
        [InlineData(DefaultOutputDir + "/OutputSuffix/filename" + OutputFileBase.CSharpOutputExtension)]
        public async Task OutputSuffix(string output)
        {
            // Arrange
            var suffix = $"-{output.Replace("/", "-")}-suffix";

            // Act
            var options = new CombineOptions()
            {
                Suffix = suffix,
                Output = output,
                OverWrite = true,
                Input = InputDir
            };

            var exitCode = await new Combiner(options).Run();

            // Assert
            Assert.Equal(0, exitCode);
            var path = string.IsNullOrEmpty(Path.GetDirectoryName(output))
                ? InputDir
                : Path.GetDirectoryName(output)!;
            var existingFiles = Directory.GetFiles(path);

            var csGeneratedFiles = existingFiles.Where(f =>
                Path.GetFileNameWithoutExtension(f).EndsWith(suffix)
                && Path.GetExtension(f) == OutputFileBase.CSharpOutputExtension);

            Assert.Single(csGeneratedFiles);
        }

        [Theory]
        [InlineData(DefaultOutputDir + "/OutputPrefixSuffix/")]
        [InlineData(DefaultOutputDir + "/OutputPrefixSuffix/filename")]
        [InlineData(DefaultOutputDir + "/OutputPrefixSuffix/filename" + OutputFileBase.CSharpOutputExtension)]
        public async Task OutputPrefixSuffix(string output)
        {
            // Arrange
            var prefix = $"preprefix-{output.Replace("/", "-")}-";
            var suffix = $"-{output.Replace("/", "-")}-suffixfix";

            // Act
            var options = new CombineOptions()
            {
                Prefix = prefix,
                Suffix = suffix,
                Output = output,
                OverWrite = true,
                Input = InputDir
            };

            var exitCode = await new Combiner(options).Run();

            // Assert
            Assert.Equal(0, exitCode);
            var path = string.IsNullOrEmpty(Path.GetDirectoryName(output))
                ? InputDir
                : Path.GetDirectoryName(output)!;
            var existingFiles = Directory.GetFiles(path);

            var csGeneratedFiles = existingFiles.Where(f =>
                Path.GetFileNameWithoutExtension(f).StartsWith(prefix)
                && Path.GetFileNameWithoutExtension(f).EndsWith(suffix)
                && Path.GetExtension(f) == OutputFileBase.CSharpOutputExtension);

            Assert.Single(csGeneratedFiles);
        }

        [Fact]
        public async Task NoOutputAndInputFile()
        {
            // Arrange
            var options = new CombineOptions()
            {
                Input = $"{InputDir}/ParsingException.cs",
                Suffix = CombinerTestsFixture.DefaultSuffix,
                Prefix = nameof(NoOutputAndInputFile),
                OverWrite = true,
            };

            // Act
            var exitCode = await new Combiner(options).Run();

            // Assert
            Assert.Equal(0, exitCode);

            var existingFiles = Directory.GetFiles(InputDir);

            var zipFiles = existingFiles.Where(f => Path.GetExtension(f) == OutputFileBase.CSharpOutputExtension
                                                && Path.GetFileNameWithoutExtension(f).Contains(options.Suffix)
                                                && Path.GetFileNameWithoutExtension(f).Contains(options.Prefix));

            Assert.NotEmpty(zipFiles);
        }

        private static DateTime? ParseDateTimeFromFileName(string fileName)
        {
            return DateTime.TryParseExact(fileName, UniqueIdGenerator.DateFormat, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out var date)
                ? (DateTime?)date
                : null;
        }
    }
}