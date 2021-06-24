using System;
using System.IO;
using USFMConverter.Core.Data;
using USFMToolsSharp.Models.Markers;
using USFMToolsSharp.Renderers.Docx;

namespace USFMConverter.Core.Render
{
    public class RenderDocx : RenderDocument
    {
        public RenderDocx()
        {

        }

        public RenderDocx(Action<double> updateProgress)
            : base(updateProgress)
        {
            
        }

        private DocxConfig BuildDocxConfig(RenderFormat format)
        {
            return new();
        }

        public override void Render(Project project, USFMDocument usfm)
        {
            var config = BuildDocxConfig(project.FormatOptions);
            var renderer = new DocxRenderer(config);
            var document = renderer.Render(usfm);

            using (Stream outputStream = File.Create(project.OutputFile.FullName))
            {
                document.Write(outputStream);
            }
        }
    }
}
