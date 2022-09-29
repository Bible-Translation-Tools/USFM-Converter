using System;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Data;
using USFMToolsSharp.Models.Markers;

namespace USFMConverter.Core.Render
{
    public interface Renderable
    {
        public void Render(Project project, string outputPath, USFMDocument usfmDoc);
    }
}
