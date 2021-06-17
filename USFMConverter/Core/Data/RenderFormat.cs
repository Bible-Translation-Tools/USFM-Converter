using USFMConverter.Core.ConstantValue;

namespace USFMConverter.Core.Data
{
    public class RenderFormat
    {
        public double LineSpacing { get; set; }
        public TextAlignment TextAlign { get; set; }
        public bool LeftToRight { get; set; } = true;
        public int FontSize { get; set; }
        public int ColumnCount { get; set; }
        public bool ChapterBreak { get; set; }
        public bool VerseBreak { get; set; }
        public bool NoteTaking { get; set; }
        public bool TableOfContents { get; set; }
    }
}
