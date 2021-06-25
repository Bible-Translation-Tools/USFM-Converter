using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace USFMConverter.UI.Pages
{
    public partial class ProjectDetailScreen : UserControl
    {
        private OptionView optionView;
        private ProgressBar progressBar;
        private Button openOptionBtn;
        
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

        private void UpdateProgressBar(double value)
        {
            progressBar.Value = value;

            if (progressBar.Value == 100)
            {
                // finish conversion
            }
        }
    }
}