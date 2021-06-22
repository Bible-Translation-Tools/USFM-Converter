using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USFMConverter.Core.ConstantValue;

namespace USFMConverter.UI
{
    public class ViewData
    {
        public List<string> Files { get; set; } = new();
        
        public int TextSizeIndex { get; set; } = 0;
        public TextSize TextSize => (TextSize)TextSizeIndex;

        public int TextAlignmentIndex { get; set; } = 0;
        public TextAlignment TextAlignment => (TextAlignment)TextAlignmentIndex;

        public int LineSpacingIndex { get; set; } = 0;
        public LineSpacing LineSpacing => (LineSpacing)LineSpacingIndex;

        public int ColumnCount { get; set; } = 1;

        public bool LeftToRight { get; set; } = true;
        public bool ChapterBreak { get; set; } = false;
        public bool VerseBreak { get; set; } = false;
        public bool NoteTaking { get; set; } = false;
        public bool TableOfContents { get; set; } = false;
        
        public string OutputLocation { get; set; } = "";
    }
}
