using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace USFMConverter.UI.Pages.PartialView
{
    public class ProjectReady : UserControl
    {
        public ProjectReady()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}