using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;


using System.Collections.Generic;
using System.IO;
using USFMConverter.UI.Pages;

namespace USFMConverter
{
    public partial class MainWindow : Window
    {
        private ProjectDetailScreen projectDetailScreen;
        private OptionView optionView;
        
        Dictionary<string, IControl> Screens = new Dictionary<string, IControl>();

        public MainWindow()
        {
            InitializeComponent();
            SetCurrentScreen(nameof(ProjectDetailScreen));
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void SetCurrentScreen(string screen)
        {
            foreach (var i in this.Screens)
            {
                i.Value.IsVisible = i.Key == screen;
            }
        }
        
        public void OptionButtonClicked(object sender, RoutedEventArgs e)
        {
            this.optionView.IsVisible = true;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.projectDetailScreen = this.FindControl<ProjectDetailScreen>("ProjectDetailScreen");
            this.optionView =  this.projectDetailScreen.FindControl<OptionView>("OptionView");
            this.Screens.Add(nameof(ProjectDetailScreen), this.projectDetailScreen);
        }
    }
}