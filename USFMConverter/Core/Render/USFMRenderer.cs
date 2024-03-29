using System.IO;
using USFMConverter.Core.Data;
using USFMToolsSharp.Models.Markers;

namespace USFMConverter.Core.Render;

public class USFMRenderer: Renderable
{
    public void Render(Project project, string outputPath, USFMDocument usfmDoc)
    {
        var renderer = new USFMToolsSharp.Renderers.USFM.USFMRenderer();
        var usfm = renderer.Render(usfmDoc);
        File.WriteAllText(outputPath, usfm);
    }
}