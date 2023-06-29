using System.Collections.Generic;

namespace DotnetCombine.Options
{
    public interface IOutputFileOptions
    {
        public string Input { get; set; }

        public string? Output { get; set; }

        public bool OverWrite { get; set; }

        public IEnumerable<string> ExcludedItems { get; set; }

        public bool PrefixWithParentFolder { get; set; }

        public string? Prefix { get; set; }

        public string? Suffix { get; set; }

        public bool Verbose { get; set; }

        public void Validate();
    }
}
