using System;
using System.IO;
using USFMConverter.Core.Data;
using USFMToolsSharp.Models.Markers;
using USFMToolsSharp.Renderers.HTML;

namespace USFMConverter.Core.Render
{
    public class RenderHTML : RenderDocument
    {
        public RenderHTML()
        {

        }

        public RenderHTML(Action<double> updateProgress)
            : base(updateProgress)
        {

        }

        private HTMLConfig BuildHTMLConfig(RenderFormat format)
        {
            return new();
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
    }
}
