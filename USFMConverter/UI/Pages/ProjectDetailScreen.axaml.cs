using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using NPOI.SS.Formula.Functions;
using USFMConverter.Core;
using USFMConverter.UI.Pages.PartialView;

namespace USFMConverter.UI.Pages
{
    public partial class ProjectDetailScreen : UserControl
    {
        private OptionView optionView;
        private ProgressBar progressBar;
        private Button openOptionBtn;
        private StackPanel backgroundOverlay;
        private UserControl progressDialog;
        private Success successDialog;
        private Error errorDialog;

        public ProjectDetailScreen()
        {
            InitializeComponent();
        }

        public event EventHandler<RoutedEventArgs> ShowBackgroundOverlay
        {
            add
            {
                AddHandler(ShowBackgroundOverlayEvent, value);
            }
            remove
            {
                RemoveHandler(ShowBackgroundOverlayEvent, value);
            }
        }

        public event EventHandler<RoutedEventArgs> HideBackgroundOverlay
        {
            add
            {
                AddHandler(HideBackgroundOverlayEvent, value);
            }
            remove
            {
                RemoveHandler(HideBackgroundOverlayEvent, value);
            }
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

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            optionView = this.FindControl<OptionView>("OptionView");

            backgroundOverlay = this.FindControl<StackPanel>("OverlayBackground");

            openOptionBtn = this.Find<Button>("OptionBtn");
            openOptionBtn.AddHandler(Button.ClickEvent, OnOpenOptionClick);

            successDialog = this.FindControl<Success>("SuccessDialog");
            errorDialog = this.FindControl<Error>("ErrorDialog");
            progressDialog = this.FindControl<Progress>("ProgressDialog");
            progressBar = progressDialog.Find<ProgressBar>("ProgressBar");
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

            var context = (ViewData)DataContext;
            try
            {
                await new CoreConverter().ConvertAsync(context, UpdateProgressBar);
            }
            catch (Exception ex)
            {
                // show error dialog
            }

            // show success dialog
            progressDialog.IsVisible = false;
            successDialog.IsVisible = true;
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