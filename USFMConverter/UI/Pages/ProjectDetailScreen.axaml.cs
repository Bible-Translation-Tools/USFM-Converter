using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using USFMConverter.Core;
using USFMConverter.UI.Pages.PartialView;
using USFMConverter.Core.Util;
using System.Reflection;
using USFMConverter.Core.Data;

namespace USFMConverter.UI.Pages
{
    public partial class ProjectDetailScreen : UserControl
    {
        private OptionView optionView;
        private FileView fileView;
        private ProgressBar progressBar;
        private StackPanel backgroundOverlay;
        private UserControl progressDialog;
        private Success successDialog;
        private Error errorDialog;

        public event EventHandler<RoutedEventArgs> ShowBackgroundOverlay
        {
            add { AddHandler(ShowBackgroundOverlayEvent, value); }
            remove { RemoveHandler(ShowBackgroundOverlayEvent, value); }
        }

        public event EventHandler<RoutedEventArgs> HideBackgroundOverlay
        {
            add { AddHandler(HideBackgroundOverlayEvent, value); }
            remove { RemoveHandler(HideBackgroundOverlayEvent, value); }
        }

        public event EventHandler<RoutedEventArgs> StartNewProject
        {
            add { AddHandler(StartNewProjectEvent, value); }
            remove { RemoveHandler(StartNewProjectEvent, value); }
        }

        public static readonly RoutedEvent<RoutedEventArgs> ShowBackgroundOverlayEvent =
            RoutedEvent.Register<FileView, RoutedEventArgs>(
                nameof(ShowBackgroundOverlay),
                RoutingStrategies.Bubble
            );

        public static readonly RoutedEvent<RoutedEventArgs> HideBackgroundOverlayEvent =
            RoutedEvent.Register<FileView, RoutedEventArgs>(
                nameof(HideBackgroundOverlay),
                RoutingStrategies.Bubble
            );

        public static readonly RoutedEvent<RoutedEventArgs> StartNewProjectEvent =
            RoutedEvent.Register<FileView, RoutedEventArgs>(
                nameof(StartNewProject),
                RoutingStrategies.Bubble
            );

        public ProjectDetailScreen()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            SetAppVersion();

            optionView = this.FindControl<OptionView>("OptionView");
            fileView = this.FindControl<FileView>("FileView");

            backgroundOverlay = this.FindControl<StackPanel>("OverlayBackground");

            successDialog = this.FindControl<Success>("SuccessDialog");
            successDialog.AddHandler(StartNewProjectEvent, OnStartNewProject);

            errorDialog = this.FindControl<Error>("ErrorDialog");
            errorDialog.AddHandler(StartNewProjectEvent, OnStartNewProject);

            progressDialog = this.FindControl<Progress>("ProgressDialog");
            progressBar = progressDialog.FindControl<ProgressBar>("ProgressBar");
        }

        private void SetAppVersion()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;
            this.FindControl<TextBlock>("AppVersion").Text =
                $"(v{version.Major}.{version.Minor}.{version.Revision})";
        }

        private void OnOpenOptionClick(object? sender, RoutedEventArgs e)
        {
            optionView.IsVisible = true;
        }

        private async void OnConvertStart(object? sender, RoutedEventArgs e)
        {
            //show progress dialog
            ShowOverlay();
            progressDialog.IsVisible = true;
            progressBar.Value = 0;

            var context = (ViewData) DataContext;
            try
            {
                await new CoreConverter().ConvertAsync(context, UpdateProgressBar);
            }
            catch (Exception ex)
            {
                // show error dialog
                progressDialog.IsVisible = false;
                errorDialog.IsVisible = true;
                errorDialog.DataContext = string.Format("{0}\n({1})", ex.Message, ex.GetType());
                return;
            }

            // show success dialog
            progressDialog.IsVisible = false;
            successDialog.IsVisible = true;
        }

        private void OnStartNewProject(object? sender, RoutedEventArgs e)
        {
            // Only remove files, keep settings
            var dataContext = (ViewData)DataContext;
            dataContext.Files.Clear();

            // reset assign data context to update UI automatically 
            ((Window)this.VisualRoot).DataContext = null;
            ((Window)this.VisualRoot).DataContext = dataContext;
            fileView.UpdateProjectStatus();
            fileView.UpdateCounter();
        }

        private void OnOpenFile(object? sender, RoutedEventArgs e)
        {
            string path = ((ViewData) DataContext).OutputFileLocation;
            try
            {
                FileSystem.OpenFile(path);
            }
            catch (Exception ex)
            {
                // show error dialog
                successDialog.IsVisible = false;
                errorDialog.IsVisible = true;
                errorDialog.DataContext = string.Format("{0}\n({1})", ex.Message, ex.GetType());
                return;
            }
        }

        private void OnOpenFolder(object? sender, RoutedEventArgs e)
        {
            string path = ((ViewData) DataContext).OutputFileLocation;
            try
            {
                FileSystem.OpenFileLocation(path);
            }
            catch (Exception ex)
            {
                // show error dialog
                successDialog.IsVisible = false;
                errorDialog.IsVisible = true;
                errorDialog.DataContext = string.Format("{0}\n({1})", ex.Message, ex.GetType());
                return;
            }
        }

        /// <summary>
        /// Set the value of progress bar. Ranging between 0-100
        /// </summary>
        /// <param name="value"></param>
        private void UpdateProgressBar(double value)
        {
            progressBar.Value = value;
        }

        private void ShowOverlay(object? sender, RoutedEventArgs e)
        {
            ShowOverlay();
        }

        private void ShowOverlay()
        {
            backgroundOverlay.IsVisible = true;
        }

        private void HideOverlay(object? sender, RoutedEventArgs e)
        {
            HideOverlay();
        }

        private void HideOverlay()
        {
            backgroundOverlay.IsVisible = false;
        }
    }
}