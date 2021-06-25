using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace USFMConverter.UI.Pages
{
    public class ProgressView : UserControl
    {
        public ProgressView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}