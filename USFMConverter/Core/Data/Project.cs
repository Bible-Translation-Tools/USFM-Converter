using System.Collections.Generic;
using System.IO;

namespace USFMConverter.Core.Data
{
    public class Project
    {
        public Project()
        {
            Files = new List<IProjectItem>();
            FormatOptions = new RenderFormat();
        }

        public string Name { get; set; } = "";
        public List<IProjectItem> Files { get; }
        public RenderFormat FormatOptions { get; set; }
    }
}
