using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using Avalonia.Interactivity;

namespace USFMConverter.UI.Pages
{
    public partial class FilesScreen : UserControl
    {
        public string TestText { get; set; } = "hello world";

        private static readonly StyledProperty<List<string>> ItemsProperty = AvaloniaProperty.Register<FilesScreen, List<string>>(nameof(Items));
        private static readonly RoutedEvent<RoutedEventArgs> FormatPageEvent = RoutedEvent.Register<FilesScreen, RoutedEventArgs>(nameof(FormatPage), RoutingStrategies.Bubble);
        public List<string> Items
        {
            get => GetValue(ItemsProperty);
            set
            {
                SetValue(ItemsProperty, value);
                this.DataContext = new RandomViewModel() { Files = value };
            }
        }
        
        public event EventHandler<RoutedEventArgs> FormatPage
        {
            add => AddHandler(FormatPageEvent, value); 
            remove => AddHandler(FormatPageEvent, value);
        }

        public FilesScreen()
        {
            InitializeComponent();
            DataContext = new RandomViewModel()
            {
                Files = this.Items
            };
        }

        public void FormatPage_Click(object sender, RoutedEventArgs e)
        {
            var format_event = new RoutedEventArgs(FormatPageEvent);
            RaiseEvent(format_event);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    public class RandomViewModel
    {
        public string TestText { get; set; } = "Hello world";
        public List<string> Files { get; set; } = new() { "1", "2", "3" };
    }
}
