using System.Collections.Generic;
using System.IO;
using System.Linq;
using USFMConverter.Core.ConstantValue;
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

        public void Convert(ViewData viewData)
        {
            var files = viewData.Files.Select(f => new FileInfo(f));
            projectBuilder.AddFiles(files.ToList());
            projectBuilder.SetFontSize(viewData.FontSize);
            projectBuilder.SetTextAlignment(viewData.TextAlignment);
            projectBuilder.SetTextDirection(viewData.LeftToRight);
            projectBuilder.SetLineSpacing(viewData.LineSpacing);
            projectBuilder.SetColumns(viewData.ColumnCount);
            projectBuilder.SetChapterBreak(viewData.ChapterBreak);
            projectBuilder.SetVerseBreak(viewData.VerseBreak);
            projectBuilder.EnableNoteTaking(viewData.NoteTaking);
            projectBuilder.EnableTableOfContents(viewData.TableOfContents);

            var project = projectBuilder.Build();
        }
    }
}
