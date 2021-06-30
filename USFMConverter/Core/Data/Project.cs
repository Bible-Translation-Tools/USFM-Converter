using System.Collections.Generic;
using System.IO;

namespace USFMConverter.Core.Data
{
    public class Project
    {
        public Project()
        {
            Files = new List<string>();
        }

        public string Name { get; set; } = "";
        public List<string> Files { get; }
        public FileInfo OutputFile { get; set; }
        public RenderFormat FormatOptions { get; set; }
    }
}
