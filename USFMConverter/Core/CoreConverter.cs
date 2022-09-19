using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Data;
using USFMConverter.Core.Render;
using USFMConverter.Core.Util;
using USFMConverter.UI;
using USFMToolsSharp.Models.Markers;

namespace USFMConverter.Core
{
    public class CoreConverter
    {
        public static ICollection<string> supportedExtensions = new List<string> {
            ".usfm", ".sfm"
        };

        public async Task ConvertAsync(ViewData viewData, Action<double> progressCallback)
        {
            if (!viewData.IndividualFiles)
            {
                FileSystem.CheckWritePermission(viewData.OutputPath);
            }

            var fileFormatName = viewData.OutputFileFormat.Tag.ToString();
            var fileFormat = Enum.Parse<FileFormat>(fileFormatName);

            var project = BuildProject(viewData);
            Renderable renderer;
            switch (fileFormat)
            {
                case FileFormat.DOCX:
                    renderer = new DocxRenderer();
                    break;
                case FileFormat.HTML:
                    renderer = new HTMLRenderer();
                    break;
                case FileFormat.USFM:
                    renderer = new USFMRenderer();
                    break;
                default:
                    throw new ArgumentException("Output file format is not supported");
            }

            var usfmDocuments = await FileSystem.LoadUSFMsAsync(project.Files, progressCallback);
            if (viewData.IndividualFiles)
            {
                var extension = viewData.OutputFileFormat.Tag.ToString().ToLower();
                foreach (var (path,document) in usfmDocuments)
                {
                    var fileName = $"{Path.GetFileNameWithoutExtension(path)}.{extension}";
                    var tocs = document.GetChildMarkers<TOC3Marker>();
                    if (tocs.Count != 0)
                    {
                        fileName = $"{tocs[0].BookAbbreviation.ToLower()}.{extension}";
                    }
                    renderer.Render(project, Path.Join(viewData.OutputPath, fileName), document );
                }

                return;
            }
            renderer.Render(project, viewData.OutputPath, MergeUSFM(usfmDocuments.Select(i => i.document)));
        }

        private USFMDocument MergeUSFM(IEnumerable<USFMDocument> input)
        {
            var output = new USFMDocument();
            foreach (var file in input)
            {
                output.Insert(file);
            }

            return output;
        }

        private Project BuildProject(ViewData viewData)
        {
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

            var project = new Project
            {
                FormatOptions = new RenderFormat
                {
                    TextSize = textSize,
                    LineSpacing = lineSpacing,
                    TextAlign = textAlignment,
                    LeftToRight = viewData.LeftToRight,
                    ColumnCount = viewData.ColumnCount,
                    ChapterBreak = viewData.ChapterBreak,
                    VerseBreak = viewData.VerseBreak,
                    NoteTaking = viewData.NoteTaking,
                    TableOfContents = viewData.TableOfContents,
                },
            };

            project.Files.AddRange(viewData.Files);

            return project;
        }
    }
}
