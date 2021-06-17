using System.Collections.Generic;
using System.IO;

namespace USFMConverter.Core.Data
{
    public class Project
    {
        public Project()
        {
            Files = new List<FileInfo>();
        }

        public string Name { get; set; } = "";
        public List<FileInfo> Files { get; }
        public FileInfo OutputFile { get; set; }
        public RenderFormat FormatOptions { get; set; }
    }
}
