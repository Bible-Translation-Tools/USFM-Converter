using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace USFMConverter.UI.Pages
{
    public partial class ProjectDetailScreen : UserControl
    {
        private readonly ComboBox _combo = new ComboBox();
        // private Button optionBtn;
        // private UserControl optionView;

        private static readonly StyledProperty<List<string>> ItemsProperty =
            AvaloniaProperty.Register<ProjectDetailScreen, List<string>>(nameof(Items));

        public List<string> Items
        {
            get => GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public ProjectDetailScreen()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            // optionBtn = this.Find<Button>("OptionBtn");
            // optionBtn.AddHandler(Button.ClickEvent, OnOptionClick);

            // optionView = this.FindControl<UserControl>("OptionView");
        }

        // private void OnOptionClick(object? sender, RoutedEventArgs e)
        // {
        //     optionView.IsVisible = true;
        // }
    }
}