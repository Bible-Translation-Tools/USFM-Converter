using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using USFMConverter.Core.Util;

namespace USFMConverter.UI.Pages.PartialView
{
    public partial class Success : UserControl
    {

        public Success()
        {
            InitializeComponent();
            this.OpenFileButton = this.FindControl<Button>("OpenFileButton");
        }

        public event EventHandler<RoutedEventArgs> OpenFile
        {
            add
            {
                AddHandler(OpenFileEvent, value);
            }
            remove
            {
                RemoveHandler(OpenFileEvent, value);
            }
        }

        public event EventHandler<RoutedEventArgs> OpenFolder
        {
            add
            {
                AddHandler(OpenFolderEvent, value);
            }
            remove
            {
                RemoveHandler(OpenFolderEvent, value);
            }
        }

        public static readonly RoutedEvent<RoutedEventArgs> OpenFileEvent =
            RoutedEvent.Register<FileView, RoutedEventArgs>(
                nameof(OpenFile),
                RoutingStrategies.Bubble
            );

        public static readonly RoutedEvent<RoutedEventArgs> OpenFolderEvent =
            RoutedEvent.Register<FileView, RoutedEventArgs>(
                nameof(OpenFolder),
                RoutingStrategies.Bubble
            );


        private bool _hideOpenFileButton;
        public bool HideOpenFileButton
        {
            get => _hideOpenFileButton;
            set
            {
                _hideOpenFileButton = value;
                this.OpenFileButton.IsVisible = !value;
            }
        }

        private Button OpenFileButton;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOpenFileClick(object? sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OpenFileEvent));
        }

        private void OnOpenFolderClick(object? sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OpenFolderEvent));
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
