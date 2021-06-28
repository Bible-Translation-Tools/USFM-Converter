using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace USFMConverter.UI.Pages.PartialView
{
    public partial class Error : UserControl
    {
        private Button newProjectBtn;
        private Button closeBtn;

        public Error()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            newProjectBtn = this.FindControl<Button>("NewProjectBtn");
            newProjectBtn.AddHandler(Button.ClickEvent, OnNewProjectClick);
            
            closeBtn = this.FindControl<Button>("CloseBtn");
            closeBtn.AddHandler(Button.ClickEvent, OnCloseDialogClick);
        }

        private void OnNewProjectClick(object? sender, RoutedEventArgs e)
        {
            this.IsVisible = false;
            ((Window)this.VisualRoot).DataContext = new ViewData();
            RaiseEvent(new RoutedEventArgs(FileView.ProjectStatusChangeEvent));
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
