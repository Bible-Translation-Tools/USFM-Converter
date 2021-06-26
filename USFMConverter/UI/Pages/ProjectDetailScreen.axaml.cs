using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using USFMConverter.Core;

namespace USFMConverter.UI.Pages
{
    public partial class ProjectDetailScreen : UserControl
    {
        private OptionView optionView;
        private ProgressBar progressBar;
        private Button openOptionBtn;
        private UserControl modalDialog;
        
        public ProjectDetailScreen()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.optionView = this.FindControl<OptionView>("OptionView");
            
            openOptionBtn = this.Find<Button>("OptionBtn");
            openOptionBtn.AddHandler(Button.ClickEvent, OnOpenOptionClick);
        }
        

        private void OnOpenOptionClick(object? sender, RoutedEventArgs e)
        {
            this.optionView.IsVisible = true;
        }

        private async void OnConvertStart(object? sender, RoutedEventArgs e)
        {
            modalDialog.IsVisible = true;
            progressBar.Value = 0;

            var context = (ViewData)DataContext;
            try
            {
                await new CoreConverter().ConvertAsync(context, UpdateProgressBar);
            }
            catch (Exception ex)
            {

            }

            // show success dialog
        }

        /// <summary>
        /// Set the value of progress bar. Max value is 100
        /// </summary>
        /// <param name="value"></param>
        private void UpdateProgressBar(double value)
        {
            progressBar.Value = value;
        }
    }
}