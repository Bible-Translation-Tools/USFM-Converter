using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USFMConverter.Core.Data
{
    public class Project
    {
        public Project()
        {
            Files = new List<FileInfo>();
        }

        public string Name { get; set; }
        public IList<FileInfo> Files { get; }
        public FileInfo OutputFile { get; set; }
        public RenderFormat FormatOptions { get; set; }
    }
}
