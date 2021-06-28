using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;

namespace USFMConverter.UI.Pages.PartialView
{
    public partial class Success : UserControl
    {
        private Button showFileBtn;
        private Button newProjectBtn;
        private Button closeBtn;

        public Success()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            showFileBtn = this.FindControl<Button>("ShowFileBtn");
            showFileBtn.AddHandler(Button.ClickEvent, OnShowFileClick);

            newProjectBtn = this.FindControl<Button>("NewProjectBtn");
            newProjectBtn.AddHandler(Button.ClickEvent, OnNewProjectClick);

            closeBtn = this.FindControl<Button>("CloseBtn");
            closeBtn.AddHandler(Button.ClickEvent, OnCloseDialogClick);
        }

        private void OnShowFileClick(object? sender, RoutedEventArgs e)
        {
            string path = ((ViewData)DataContext).OutputFileLocation;

            Process.Start("explorer.exe", @"/select," + path);
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
