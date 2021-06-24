using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace USFMConverter.UI.Pages
{
    public partial class ProjectDetailScreen : UserControl
    {
        
        private Button openOptionBtn;

        public static readonly RoutedEvent<RoutedEventArgs> OptionOpenEvent =
            RoutedEvent.Register<FileView, RoutedEventArgs>(nameof(OptionOpen), RoutingStrategies.Bubble);

        public event EventHandler<RoutedEventArgs> OptionOpen
        {
            add => AddHandler(OptionOpenEvent, value);
            remove => AddHandler(OptionOpenEvent, value);
        }

        public ProjectDetailScreen()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            openOptionBtn = this.Find<Button>("OptionBtn");
            openOptionBtn.AddHandler(Button.ClickEvent, OnOpenOptionClick);
        }

        private void OnOpenOptionClick(object? sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OptionOpenEvent));
        }
    }
}