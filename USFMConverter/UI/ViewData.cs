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
        public List<string> Files { get; set; } = new() {"default.usfm"};
        public int FontSize { get; set; } = 12;
        public double LineSpacing { get; set; } = 1.0;
        public bool LeftToRight { get; set; } = true;
        public int ColumnCount { get; set; } = 1;
        public bool ChapterBreak { get; set; } = false;
        public bool VerseBreak { get; set; } = false;
        public bool NoteTaking { get; set; } = false;
        public bool TableOfContents { get; set; } = false;
        public string OutputLocation { get; set; } = "";
        public TextAlignment TextAlignment { get; set; } = TextAlignment.LEFT;
    }
}