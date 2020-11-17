﻿using System;
using CommandLine;
using System.Collections.Generic;

namespace DotnetCombine.Options
{
    [Verb("zip", isDefault: false, HelpText = "Zips multiple files")]
    public class ZipOptions
    {
        [Value(0, MetaName = "input", Required = true, HelpText = @"
Input path (file or directory).")]
        public string Input { get; set; } = null!;

        [Option(shortName: 'o', longName: "output", Required = false, HelpText = @"
Output path (file or directory).
If no path is provided, the file will be created in the input directory.
If no filename is provided, a unique name based on the generation date will be used.")]
        public string? Output { get; set; }

        [Option(longName: "extensions", Required = false, Separator = ';', Default = new[] { ".sln", ".csproj", ".cs" }, HelpText = @"
File extensions to include.")]
        public IEnumerable<string> Extensions { get; set; } = new[] { ".cs", ".csproj", ".sln" };

        [Option(shortName: 'f', longName: "overwrite", Required = false, Default = false, HelpText = @"
Replace the output zip file if it exists. Default behavior is updating it.")]
        public bool OverWrite { get; set; }

        [Option(longName: "exclude", Required = false, Separator = ';', Default = new[] { "bin/", "obj/" }, HelpText = @"
Excluded files and directories, separated by semicolons (;).
No regex or globalling is supported (yet), sorry!")]
        public IEnumerable<string> ExcludedItems { get; set; } = new[] { "bin/", "obj/" };

        [Option(shortName: 'p', longName: "prefix", Required = false, HelpText = @"
Prefix for the output file.")]
        public string? Prefix { get; set; }

        [Option(shortName: 's', longName: "suffix", Required = false, HelpText = @"
Suffix for the output file.")]
        public string? Suffix { get; set; }

        public void Validate()
        {
            if (Input is null)
            {
                throw new ArgumentException($"{nameof(Input)} is required");
            }
        }
    }
}