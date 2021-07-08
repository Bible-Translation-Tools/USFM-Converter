using USFMConverter.UI;

namespace USFMConverter.Core.Data
{
    public class Setting
    {
        public int TextSizeIndex { get; set; }
        public int LineSpacingIndex { get; set; }
        public int ColumnCountIndex { get; set; }
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
            TextSizeIndex = dataContext.SelectedTextSizeIndex;
            LineSpacingIndex = dataContext.SelectedLineSpacingIndex;
            ColumnCountIndex = dataContext.ColumnCount;
            Justified = dataContext.Justified;
            LeftToRight = dataContext.LeftToRight;
            ChapterBreak = dataContext.ChapterBreak;
            VerseBreak = dataContext.VerseBreak;
            NoteTaking = dataContext.NoteTaking;
            TableOfContents = dataContext.TableOfContents;
        }
    }
}
