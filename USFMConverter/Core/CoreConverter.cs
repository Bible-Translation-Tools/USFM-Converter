using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Data;
using USFMConverter.Core.Render;
using USFMConverter.Core.Util;
using USFMConverter.UI;
using USFMToolsSharp;
using USFMToolsSharp.Models.Markers;

namespace USFMConverter.Core
{
    public class CoreConverter
    {

        public static ICollection<string> supportedExtensions = new List<string> { 
            ".usfm", ".txt", ".sfm" 
        };

        public async Task ConvertAsync(ViewData viewData, Action<double> progressCallback)
        {
            FileSystem.CheckWritePermission(viewData.OutputFileLocation);

            string fileFormatName = viewData.OutputFileFormat.Tag.ToString();
            var fileFormat = Enum.Parse<FileFormat>(fileFormatName);
            
            Project project = BuildProject(viewData);
            Renderable renderer;
            switch (fileFormat)
            {
                case FileFormat.DOCX:
                    renderer = new DocxRenderer();
                    break;
                case FileFormat.HTML:
                    renderer = new HTMLRenderer();
                    break;
                default:
                    throw new ArgumentException("Output file format is not supported");
            }

            var usfmDocument = await FileSystem.LoadUSFMsAsync(project.Files, progressCallback);
            renderer.Render(project, usfmDocument);

            progressCallback(100); // fills the progress bar
            await Task.Delay(300); // visible completion before transition
        }

        private Project BuildProject(ViewData viewData)
        {
            var project = new Project();
            var files = viewData.Files;

            var textSizeName = viewData.TextSize.Tag?.ToString();
            var textSize = (string.IsNullOrEmpty(textSizeName)) 
                ? TextSize.MEDIUM
                : Enum.Parse<TextSize>(textSizeName);

            var lineSpacingName = viewData.LineSpacing.Tag?.ToString();
            var lineSpacing = (string.IsNullOrEmpty(lineSpacingName))
                ? LineSpacing.SINGLE
                : Enum.Parse<LineSpacing>(lineSpacingName);

            var textAlignment = (viewData.Justified)
                ? TextAlignment.JUSTIFIED
                : TextAlignment.LEFT;

            project.Files.AddRange(files);
            project.FormatOptions.TextSize = textSize;
            project.FormatOptions.LineSpacing = lineSpacing;
            project.FormatOptions.TextAlign  = textAlignment;
            project.FormatOptions.LeftToRight = viewData.LeftToRight;
            project.FormatOptions.ColumnCount = viewData.ColumnCount;
            project.FormatOptions.ChapterBreak = viewData.ChapterBreak;
            project.FormatOptions.VerseBreak = viewData.VerseBreak;
            project.FormatOptions.NoteTaking = viewData.NoteTaking;
            project.FormatOptions.TableOfContents = viewData.TableOfContents;
            project.OutputFile = new FileInfo(viewData.OutputFileLocation);

            return project;
        }
    }
}
