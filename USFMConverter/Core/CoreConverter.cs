using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Data;
using USFMConverter.Core.Render;
using USFMConverter.UI;

namespace USFMConverter.Core
{
    public class CoreConverter
    {

        public static ICollection<string> supportedExtensions = new List<string> { 
            ".usfm", ".txt", ".sfm" 
        };

        public CoreConverter()
        {

        }

        public async Task ConvertAsync(ViewData viewData, Action<double> progressCallback)
        {
            string fileFormatName = viewData.OutputFileFormat.Tag?.ToString();
            if (fileFormatName == null)
            {
                throw new ArgumentException("Output file format is not properly specified.");
            }

            Project project = BuildProject(viewData);

            var fileFormat = Enum.Parse<FileFormat>(fileFormatName);
            RenderDocument renderer;

            switch (fileFormat)
            {
                case FileFormat.DOCX:
                    renderer = new RenderDocx();
                    break;
                case FileFormat.HTML:
                    renderer = new RenderHTML();
                    break;
                default:
                    renderer = null;
                    break;
            }

            var usfmDocument = await renderer.LoadUSFMsAsync(project.Files, progressCallback);
            renderer.Render(project, usfmDocument);

            progressCallback(100); // fills the progress bar
            await Task.Delay(500); // visible completion before transition
        }

        private Project BuildProject(ViewData viewData)
        {
            var projectBuilder = new ProjectBuilder();

            var files = viewData.Files.Select(f => new FileInfo(f));

            var textSizeName = viewData.TextSize.Tag?.ToString();
            var lineSpacing = viewData.LineSpacing.Tag?.ToString();

            var textAlignment = (viewData.Justified)
                ? TextAlignment.JUSTIFIED
                : TextAlignment.LEFT;

            projectBuilder.AddFiles(files.ToList());
            projectBuilder.SetTextSize(GetTextSize(textSizeName));
            projectBuilder.SetLineSpacing(GetLineSpacing(lineSpacing));
            projectBuilder.SetTextAlignment(textAlignment);
            projectBuilder.SetTextDirection(viewData.LeftToRight);
            projectBuilder.SetColumns(viewData.ColumnCount);
            projectBuilder.SetChapterBreak(viewData.ChapterBreak);
            projectBuilder.SetVerseBreak(viewData.VerseBreak);
            projectBuilder.EnableNoteTaking(viewData.NoteTaking);
            projectBuilder.EnableTableOfContents(viewData.TableOfContents);
            projectBuilder.SetOutputLocation(viewData.OutputFileLocation);

            return projectBuilder.Build();
        }

        private TextSize GetTextSize(string? sizeName)
        {
            if (string.IsNullOrEmpty(sizeName))
            {
                return TextSize.MEDIUM;
            }

            return Enum.Parse<TextSize>(sizeName);
        }

        private LineSpacing GetLineSpacing(string? spacingName)
        {
            if (string.IsNullOrEmpty(spacingName))
            {
                return LineSpacing.SINGLE;
            }

            return Enum.Parse<LineSpacing>(spacingName);
        }
    }
}
