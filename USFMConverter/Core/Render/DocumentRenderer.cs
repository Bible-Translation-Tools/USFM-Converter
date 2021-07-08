using System;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Data;
using USFMToolsSharp.Models.Markers;

namespace USFMConverter.Core.Render
{
    public abstract class DocumentRenderer
    {
        public USFMDocument? FrontMatter { get; set; }

        public abstract void Render(Project project, USFMDocument usfmDoc);
    }
}
