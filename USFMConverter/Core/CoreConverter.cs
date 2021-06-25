using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Render;
using USFMConverter.UI;

namespace USFMConverter.Core
{
    public class CoreConverter
    {
        private ProjectBuilder projectBuilder;

        public static ICollection<string> supportedExtensions = new List<string> { 
            ".usfm", ".txt", ".sfm" 
        };

        public CoreConverter()
        {
            projectBuilder = new ProjectBuilder();
        }

        public void Convert(ViewData viewData, Action<double> progressCallback)
        {
            var files = viewData.Files.Select(f => new FileInfo(f));
            
            var textSizeName = viewData.TextSize.Tag?.ToString();
            var lineSpacing = viewData.LineSpacing.Tag?.ToString();

            var textAlignment = (viewData.Justified)? TextAlignment.JUSTIFIED 
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

            var project = projectBuilder.Build();

            string fileFormatName = viewData.OutputFileFormat.Tag?.ToString();
            if (fileFormatName == null)
            {
                throw new ArgumentException(
                    "Output file format is not properly specified. " +
                    "Selected format: " + viewData.OutputFileFormat.Tag
                    );
            }

            var outputType = Enum.Parse<FileFormat>(fileFormatName);
            RenderDocument renderer;
            
            switch (outputType)
            {
                case FileFormat.DOCX:
                    renderer = new RenderDocx(progressCallback);
                    break;
                case FileFormat.HTML:
                    renderer = new RenderHTML();
                    break;
                default:
                    renderer = null;
                    break;
            }

            renderer.Render(project);
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
