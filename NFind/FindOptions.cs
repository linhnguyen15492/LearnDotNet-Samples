using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class FindOptions
    {
        public bool FindContainingLines { get; set; } = true;
        public bool CountMode { get; set; } = false;
        public bool ShowLineNumber { get; set; } = false;
        public bool CaseSensitive { get; set; } = true;
        public bool SkipOfflineFiles { get; set; } = true;
        public string StringToFind { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool HelpMode { get; set; } = false;
    }
}
