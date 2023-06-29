using DotnetCombine.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetCombine.Services
{

    public class Compressor : OutputFileManager
    {
        public override string OutputExtension { get { return ZipOutputExtension; } }

        private ZipOptions _options;
        public override ZipOptions options { get { return _options; } }


        public Compressor(ZipOptions options)
            : base(options)
        {
            _options = options;
        }

        public int Run()
        {
            Func<Task> actions = async () =>
            {
                var filesToInclude = FindFilesToInclude();
                GenerateZipFile(filesToInclude);
            };

            return base.Run(actions).Result;
        }

        protected override IList<string> FindFilesToInclude()
        {
            if (File.Exists(options.Input))
            {
                return new List<string> { options.Input };
            }

            var filesToExclude = options.ExcludedItems.Where(item => !Path.EndsInDirectorySeparator(item));

            var dirsToExclude = options.ExcludedItems
                .Except(filesToExclude)
                .Select(dir => Path.DirectorySeparatorChar + dir.ReplaceEndingDirectorySeparatorWithProperEndingDirectorySeparator());

            var filesToInclude = new List<string>();
            foreach (var extension in options.Extensions)
            {
                filesToInclude.AddRange(
                    Directory.GetFiles(options.Input, $"*.{extension.TrimStart('*').TrimStart('.')}", SearchOption.AllDirectories)
                        .Where(f => !dirsToExclude.Any(exclusion => $"{Path.GetDirectoryName(f)}{Path.DirectorySeparatorChar}"?.Contains(exclusion, StringComparison.OrdinalIgnoreCase) == true)
                                && !filesToExclude.Any(exclusion => string.Equals(Path.GetFileName(f), exclusion, StringComparison.OrdinalIgnoreCase))));
            }

            return filesToInclude;
        }

        private void GenerateZipFile(IList<string> filesToInclude)
        {
            var pathToTrim = Directory.Exists(options.Input)
                ? options.Input.ReplaceEndingDirectorySeparatorWithProperEndingDirectorySeparator()
                : string.Empty;

            using var fs = new FileStream(outputFilePath, options.OverWrite ? FileMode.Create : FileMode.CreateNew);
            using var zip = new ZipArchive(fs, ZipArchiveMode.Create);
            for (int i = 0; i < filesToInclude.Count; ++i)
            {
                var file = filesToInclude[i];

                if (options.Verbose)
                {
                    Console.WriteLine($"\t* [{i + 1}/{filesToInclude.Count}] Aggregating {file}");
                }

                zip.CreateEntryFromFile(file, file[pathToTrim.Length..]);
            }
        }
    }
}