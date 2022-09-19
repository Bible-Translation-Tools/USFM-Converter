using Avalonia.Controls;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using USFMConverter.Core.Data;

namespace USFMConverter.UI
{
    public class ViewData
    {
        public List<IProjectItem> Files { get; set; } = new();

        public int SelectedFormatIndex { get; set; } = 0;
        public ComboBoxItem OutputFileFormat { get; set; }

        public int SelectedTextSizeIndex { get; set; } = 0; // binding allows resetting user input
        public ComboBoxItem TextSize { get; set; }

        public int SelectedLineSpacingIndex { get; set; } = 0; // binding allows resetting user input
        public ComboBoxItem LineSpacing { get; set; }

        public int ColumnCount { get; set; } = 1;
        public bool Justified { get; set; } = false;
        public bool LeftToRight { get; set; } = true;
        public bool ChapterBreak { get; set; } = false;
        public bool VerseBreak { get; set; } = false;
        public bool NoteTaking { get; set; } = false;
        public bool TableOfContents { get; set; } = false;
        public string OutputPath { get; set; } = "";
        public bool IndividualFiles { get; set; }

        public Exception Error { get; set; }
    }
}
