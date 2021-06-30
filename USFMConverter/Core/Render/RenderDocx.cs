using System;
using System.Collections.Generic;
using System.IO;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Data;
using USFMToolsSharp.Models.Markers;
using USFMToolsSharp.Renderers.Docx;

namespace USFMConverter.Core.Render
{
    public class RenderDocx : RenderDocument
    {
        private Dictionary<TextSize, int> FontSizeMap = new()
        {
            [TextSize.SMALL] = 12,
            [TextSize.MEDIUM] = 14,
            [TextSize.LARGE] = 16
        };

        private Dictionary<LineSpacing, double> LineSpacingMap = new()
        {
            [LineSpacing.SINGLE] = 1.0,
            [LineSpacing.ONE_HALF] = 1.5,
            [LineSpacing.DOUBLE] = 2.0,
            [LineSpacing.TWO_HALF] = 2.5,
            [LineSpacing.TRIPLE] = 3.0,
        };

        private DocxConfig BuildDocxConfig(RenderFormat format)
        {
            DocxConfig config = new DocxConfig();
            config.fontSize = GetFontSize(format.TextSize);
            config.lineSpacing = GetLineSpacing(format.LineSpacing);
            config.columnCount = format.ColumnCount;
            config.rightToLeft = !format.LeftToRight;
            config.separateVerses = format.VerseBreak;
            config.separateChapters = format.ChapterBreak;
            config.renderTableOfContents = format.TableOfContents;
            
            if (format.TextAlign == TextAlignment.JUSTIFIED) {
                config.textAlign = NPOI.XWPF.UserModel.ParagraphAlignment.BOTH;
            }

            if (format.NoteTaking)
            {
                // this applies to both LTR and RTL direction
                config.marginLeft = -2;
                config.marginRight = 4;
            }

            return config;
        }

        public override void Render(Project project, USFMDocument usfm)
        {
            var config = BuildDocxConfig(project.FormatOptions);
            var renderer = new DocxRenderer(config);

            var document = renderer.Render(usfm);

            // check write permission to output location
            using (Stream outputStream = File.Create(project.OutputFile.FullName))
            {
                document.Write(outputStream);
            }
        }

        private double GetLineSpacing(LineSpacing spacing)
        {
            return LineSpacingMap[spacing];
        }

        private int GetFontSize(TextSize textSize)
        {
            return FontSizeMap[textSize];
        }
    }
}
