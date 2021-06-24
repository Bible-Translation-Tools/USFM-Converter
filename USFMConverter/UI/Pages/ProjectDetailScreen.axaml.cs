using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using USFMConverter.Core;
using USFMConverter.Core.Util;

namespace USFMConverter.UI.Pages
{
    public partial class ProjectDetailScreen : UserControl
    {
        private ProgressBar progressBar;

        public ProjectDetailScreen()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void UpdateProgressBar(double value)
        {
            progressBar.Value = value;

            if (progressBar.Value == 100)
            {
                // finish conversion
            }
        }
    }
}