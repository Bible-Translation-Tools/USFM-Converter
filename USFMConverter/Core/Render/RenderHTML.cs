﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using USFMConverter.Core.ConstantValue;
using USFMConverter.Core.Data;
using USFMToolsSharp.Models.Markers;
using USFMToolsSharp.Renderers.HTML;

namespace USFMConverter.Core.Render
{
    public class RenderHTML : RenderDocument
    {
        private List<string> TextDirectionClasses = new() { "", "rtl-direct" };

        private Dictionary<LineSpacing, string> LineSpacingClasses = new()
        {
            [LineSpacing.SINGLE] = "single-space",
            [LineSpacing.ONE_HALF] = "one-half-space",
            [LineSpacing.DOUBLE] = "double-space",
            [LineSpacing.TWO_HALF] = "two-half-space",
            [LineSpacing.TRIPLE] = "triple-space"
        };

        private Dictionary<int, string> ColumnClasses = new() {
            [1] = "", 
            [2] = "two-column" 
        };

        private Dictionary<TextAlignment, string> TextAlignmentClasses = new() { 
            [TextAlignment.LEFT] = "",
            [TextAlignment.RIGHT] = "right-align",
            [TextAlignment.CENTER] = "center-align",
            [TextAlignment.JUSTIFIED] = "justified"
        };

        private Dictionary<TextSize, string> FontSizeClasses = new()
        {
            [TextSize.SMALL] = "small-text",
            [TextSize.MEDIUM] = "med-text",
            [TextSize.LARGE] = "large-text"
        };

        public RenderHTML()
        {

        }

        private HTMLConfig BuildHTMLConfig(RenderFormat format)
        {
            HTMLConfig config = new HTMLConfig();
            var styleClasses = new List<string>();

            styleClasses.Add(GetLineSpacingStyle(format.LineSpacing));
            styleClasses.Add(GetColumnStyle(format.ColumnCount));
            styleClasses.Add(GetDirectionStyle(format.LeftToRight));
            styleClasses.Add(GetTextAlignStyle(format.TextAlign));
            styleClasses.Add(GetFontSizeStyle(format.TextSize));

            var classesToAdd = styleClasses
                .Where(s => !string.IsNullOrEmpty(s)); // filter valid class only

            config.divClasses.AddRange(classesToAdd);

            config.separateVerses = format.VerseBreak;
            config.separateChapters = format.ChapterBreak;
            config.renderTableOfContents = format.TableOfContents;

            return config;
        }

        public override void Render(Project project, USFMDocument usfmDoc)
        {
            var config = BuildHTMLConfig(project.FormatOptions);
            var renderer = new HtmlRenderer(config);

            renderer.FrontMatterHTML = GetLicenseInfo();
            renderer.InsertedFooter = GetFooterInfo();

            var htmlString = renderer.Render(usfmDoc);

            File.WriteAllText(project.OutputFile.FullName, htmlString);

            var cssFilename = Path.Combine(project.OutputFile.DirectoryName!, "style.css");
            if (!File.Exists(cssFilename))
            {
                File.Copy("style.css", cssFilename);
            }
        }

        private string GetLicenseInfo()
        {
            // Identifies License within Directory 
            string ULB_License_Doc = "insert_ULB_License.html";
            FileInfo f = new FileInfo(ULB_License_Doc);
            string licenseHTML = "";

            if (File.Exists(ULB_License_Doc))
            {
                licenseHTML = File.ReadAllText(ULB_License_Doc);
            }
            return licenseHTML;
        }

        private string GetFooterInfo()
        {
            string dateFormat = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            string footerHTML = $@"
            <div class=FooterSection>
            <table id='hrdftrtbl' border='0' cellspacing='0' cellpadding='0'>
            <div class=FooterSection>
            <table id='hrdftrtbl' border='0' cellspacing='0' cellpadding='0'>
            <tr><td>
            <div style='mso-element:footer' id=f1>
            <p class=MsoFooter></p>
            {dateFormat}
            <span style=mso-tab-count:1></span>
            <span style='mso-field-code: PAGE '></span><span style='mso-no-proof:yes'></span></span>
            <span style=mso-tab-count:1></span>
            <img alt='Creative Commons License' style='border-width:0' src='https://i.creativecommons.org/l/by-sa/4.0/88x31.png' />
            </p>
            </div>
            </td></tr>
            </table>
            </div> ";
            return footerHTML;
        }

        private string GetLineSpacingStyle(LineSpacing spacing)
        {
            return LineSpacingClasses[spacing];
        }

        private string GetColumnStyle(int columnCount)
        {
            return ColumnClasses[columnCount];
        }

        private string GetDirectionStyle(bool leftToRight)
        {
            if (!leftToRight)
            {
                return TextDirectionClasses[1];
            }

            return TextDirectionClasses[0];
        }

        private string GetTextAlignStyle(TextAlignment alignment)
        {
            return TextAlignmentClasses[alignment];
        }

        private string GetFontSizeStyle(TextSize size)
        {
            return FontSizeClasses[size];
        }
    }
}
