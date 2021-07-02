using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using USFMConverter.Core.Util;

namespace USFMConverter.UI.Pages.PartialView
{
    public partial class Success : UserControl
    {

        public Success()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOpenFileClick(object? sender, RoutedEventArgs e)
        {
            string path = ((ViewData)DataContext).OutputFileLocation;
            FileSystem.OpenFile(path);
        }

        private void OnShowFileClick(object? sender, RoutedEventArgs e)
        {
            string path = ((ViewData)DataContext).OutputFileLocation;
            FileSystem.OpenFileLocation(path);
        }

        private void OnNewProjectClick(object? sender, RoutedEventArgs e)
        {
            this.IsVisible = false;
            RaiseEvent(new RoutedEventArgs(ProjectDetailScreen.StartNewProjectEvent));
            HideOverlayBackground();
        }

        private void OnCloseDialogClick(object? sender, RoutedEventArgs e)
        {
            this.IsVisible = false;
            HideOverlayBackground();
        }

        private void HideOverlayBackground()
        {
            RaiseEvent(new RoutedEventArgs(ProjectDetailScreen.HideBackgroundOverlayEvent));
        }
    }
}
