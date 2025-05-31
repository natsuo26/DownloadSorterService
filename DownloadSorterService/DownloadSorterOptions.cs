using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadSorterService
{
    public class DownloadSorterOptions
    {
        public string DownloadPath { get; set; } = string.Empty;
        public Dictionary<string, List<string>> FileCategories { get; set; } = new();
        public List<string> ExcludedExtensions { get; set; } = new(); 
    }
}
