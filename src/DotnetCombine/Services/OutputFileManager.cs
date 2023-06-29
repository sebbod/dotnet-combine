using DotnetCombine.Model;
using DotnetCombine.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCombine.Services
{
    public abstract class OutputFileManager
    {
        #region OutputExtension
        public const string CSharpOutputExtension = ".cs";
        public const string ZipOutputExtension = ".zip";
        #endregion

        public abstract string OutputExtension { get; }

        private ICombineOptions _options;
        public abstract ICombineOptions options { get; }

        public string fileName { get; private set; } = null!;

        public string basePath { get; private set; } = null!;

        public string outputFilePath { get; private set; } = null!;


        public OutputFileManager(ICombineOptions options)
        {
            this._options = options;
            ValidateInputAndGenerateMetaData();
        }

        protected abstract IList<string> FindFilesToInclude();

        protected async Task<int> Run(Func<Task> actionsFromChilClass)
        {
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            try
            {
                ValidateInputAndGenerateMetaData();

                await actionsFromChilClass();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Output file: {outputFilePath}");
                return 0;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
#if DEBUG
                Console.WriteLine(e.GetType() + Environment.NewLine + e.StackTrace);
#endif
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (OperatingSystem.IsWindows())
                {
                    Console.WriteLine($"If you intended to use '{outputFilePath}' as output file, " +
                        "try running `dotnet-combine single-file` from an elevated prompt (using \"Run as Administrator\").");
                }
                else
                {
                    Console.WriteLine($"If you intended to use '{outputFilePath}' as output file, " +
                        "try running `dotnet-combine single-file` as superuser (i.e. using 'sudo').");
                }

                return 1;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
#if DEBUG
                Console.WriteLine(e.GetType() + Environment.NewLine + e.StackTrace);
#endif
                return 1;
            }
            finally
            {
                Console.ResetColor();

#if DEBUG
                Console.WriteLine($"Total time: {sw.ElapsedMilliseconds}ms");
#endif
            }
        }

        /// <summary>
        /// TOTO add property change on all properties of CombineOptions and ZipOptions to call ValidateInputAndGenerateMetaData just if needed (when options hasChanged)
        /// </summary>
        protected void ValidateInputAndGenerateMetaData()
        {
            _options.Validate();

            GenerateMetaData();

            if (!_options.OverWrite && File.Exists(outputFilePath))
            {
                throw new CombineException(
                    $"The file {outputFilePath} already exists{Environment.NewLine}" +
                    $"Did you mean to set --overwrite to true?{Environment.NewLine}" +
                    "You can also leave --output empty to always have a new one generated (and maybe use --prefix or --suffix to identify it).");
            }
        }

        private void GenerateMetaData()
        {
            string composeFileName(string fileNameWithoutExtension) =>
                (_options.Prefix ?? string.Empty) +
                fileNameWithoutExtension +
                (_options.Suffix ?? string.Empty) +
                OutputExtension;

            fileName = composeFileName(UniqueIdGenerator.UniqueId());
            basePath = File.Exists(_options.Input)
                            ? Path.GetDirectoryName(_options.Input) ?? throw new CombineException($"{_options.Input} parent dir not found, try providing an absolute or relative path")
                            : _options.Input!;

            if (_options.Output is not null)
            {
                if (Path.EndsInDirectorySeparator(_options.Output))
                {
                    basePath = _options.Output.ReplaceEndingDirectorySeparatorWithProperEndingDirectorySeparator();
                    Directory.CreateDirectory(basePath);
                }
                else
                {
                    var directoryName = Path.GetDirectoryName(_options.Output);

                    basePath = string.IsNullOrEmpty(directoryName)
                        ? _options.Input + Path.DirectorySeparatorChar
                        : Directory.CreateDirectory(directoryName).FullName;

                    fileName = composeFileName(Path.GetFileNameWithoutExtension(_options.Output));
                }
            }

            if (_options.PrefixWithParentFolder)
            {
                string folderName = new DirectoryInfo(basePath).Name;
                fileName = folderName + "_" + fileName;
            }

            outputFilePath = Path.Combine(basePath, fileName);
        }
    }
}
