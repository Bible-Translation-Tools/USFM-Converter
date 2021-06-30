using System.Collections.Generic;
using System.IO;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Data;
using USFMToolsSharp.Models.Markers;
using USFMToolsSharp.Renderers.Docx;

namespace USFMConverter.Core.Render
{
    public class DocxRenderer : Renderable
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
            DocxConfig config = new DocxConfig
            {
                fontSize = GetFontSize(format.TextSize),
                lineSpacing = GetLineSpacing(format.LineSpacing),
                columnCount = format.ColumnCount,
                rightToLeft = !format.LeftToRight,
                separateVerses = format.VerseBreak,
                separateChapters = format.ChapterBreak,
                renderTableOfContents = format.TableOfContents
            };
            
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
            var renderer = new USFMToolsSharp.Renderers.Docx.DocxRenderer(config);

            var document = renderer.Render(usfm);

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
