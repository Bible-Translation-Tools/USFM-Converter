using USFMConverter.UI;

namespace USFMConverter.Core.Data.Serializer
{
    public class Setting
    {
        public int TextSizeIndex { get; set; }
        public int LineSpacingIndex { get; set; }
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
        public Setting(ViewData dataContext)
        {
            TextSizeIndex = dataContext.SelectedTextSizeIndex;
            LineSpacingIndex = dataContext.SelectedLineSpacingIndex;
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
