using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace USFMConverter.UI.Pages
{
    public class SuccessView : UserControl
    {
        public SuccessView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}