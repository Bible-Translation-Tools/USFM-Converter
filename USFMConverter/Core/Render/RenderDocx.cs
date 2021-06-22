using System.IO;
using USFMConverter.Core.Data;
using USFMToolsSharp.Renderers.Docx;

namespace USFMConverter.Core.Render
{
    public class RenderDocx : RenderDocument
    {
        public RenderDocx()
        {

        }

        private DocxConfig BuildDocxConfig(RenderFormat format)
        {
            return new();
        }

        public override void Render(Project project)
        {
            var config = BuildDocxConfig(project.FormatOptions);
            var usfm = LoadUSFM(project.Files);

            var renderer = new DocxRenderer(config);
            var document = renderer.Render(usfm);

            using (Stream outputStream = File.Create(project.OutputFile.FullName))
            {
                document.Write(outputStream);
            }
        }
    }
}
