using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace USFMConverter.UI.Pages.PartialView
{
    public partial class Error : UserControl
    {

        public Error()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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
