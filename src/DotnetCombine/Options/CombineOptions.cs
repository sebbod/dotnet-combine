﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotnetCombine.Options
{

    [Verb("single-file", isDefault: false, HelpText = "Combines multiple source code files (.cs) into a single one.")]
    public class CombineOptions : IOutputFileOptions
    {
        [Value(0, MetaName = "input", Required = true, HelpText = @"
Input path.")]
        public string Input { get; set; } = null!;

        [Option(shortName: 'o', longName: "output", Required = false, HelpText = @"
Output path (file or dir).
If no dir path is provided (i.e. --output file.cs), the file will be created in the input dir.
If no filename is provided (i.e. --output dir/), a unique name will be used.")]
        public string? Output { get; set; }

        [Option(shortName: 'f', longName: "overwrite", Required = false, Default = false, HelpText = @"
Overwrites the output file (if it exists).")]
        public bool OverWrite { get; set; }

        [Option(longName: "exclude", Required = false, Separator = ';', Default = new[] { "bin/", "obj/" }, HelpText = @"
Excluded files and directories, separated by semicolons (;).")]
        public IEnumerable<string> ExcludedItems { get; set; } = new[] { "bin/", "obj/" };

        [Option(shortName: 'P', longName: "prefixWithParentFolder", Required = false, Default = true, HelpText = @"
Prefix with the folder name for the output file.")]
        public bool PrefixWithParentFolder { get; set; }

        [Option(shortName: 'p', longName: "prefix", Required = false, HelpText = @"
Prefix for the output file.")]
        public string? Prefix { get; set; }

        [Option(shortName: 's', longName: "suffix", Required = false, HelpText = @"
Suffix for the output file")]
        public string? Suffix { get; set; }

        [Option(shortName: 'v', longName: "verbose", Required = false, Default = false, HelpText = @"
Verbose output. Shows combined files, progress, etc.")]
        public bool Verbose { get; set; }

        [Option(longName: "format", Required = false, Default = false, HelpText = @"
Formats output file. Enabling it slows down file generation process.")]
        public bool Format { get; set; }

        public void Validate()
        {
            if (Input is null)
            {
                throw new ArgumentException($"{nameof(Input)} is required");
            }

            if (!Directory.Exists(Input) && !File.Exists(Input))
            {
                throw new ArgumentException($"Could not find path '{Input}'.");
            }
        }
    }
}