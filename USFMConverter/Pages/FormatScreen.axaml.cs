using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace QuickUSFMConverter.Screens
{
    public class FormatOptionsScreen : UserControl
    {
        public FormatOptionsScreen()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}