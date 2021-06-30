using System;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Data;
using USFMToolsSharp.Models.Markers;

namespace USFMConverter.Core.Render
{
    public abstract class Renderable
    {
        public abstract void Render(Project project, USFMDocument usfmDoc);
    }
}
