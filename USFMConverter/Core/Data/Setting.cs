using USFMConverter.UI;

namespace USFMConverter.Core.Data
{
    public class Setting
    {
        public int TextSize { get; set; }
        public int LineSpacing { get; set; }
        public int ColumnCount { get; set; }
        public bool Justified { get; set; }
        public bool LeftToRight { get; set; }
        public bool ChapterBreak { get; set; }
        public bool VerseBreak { get; set; }
        public bool NoteTaking { get; set; }
        public bool TableOfContents { get; set; }

        public Setting()
        {
            
        }
        public Setting(ViewData? dataContext)
        {
            if (dataContext == null) return;
            TextSize = dataContext.SelectedTextSizeIndex;
            LineSpacing = dataContext.SelectedLineSpacingIndex;
            ColumnCount = dataContext.ColumnCount;
            Justified = dataContext.Justified;
            LeftToRight = dataContext.LeftToRight;
            ChapterBreak = dataContext.ChapterBreak;
            VerseBreak = dataContext.VerseBreak;
            NoteTaking = dataContext.NoteTaking;
            TableOfContents = dataContext.TableOfContents;
        }
    }
}
