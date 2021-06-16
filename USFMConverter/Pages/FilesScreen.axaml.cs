using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace USFMConverter.Pages
{
    public partial class FilesScreen : UserControl
    {
        public string TestText { get; set; } = "hello world";

        public static readonly StyledProperty<List<string>> ItemsProperty = AvaloniaProperty.Register<FilesScreen, List<string>>(nameof(Items));
        public List<string> Items
        {
            get => GetValue(ItemsProperty);
            set
            {
                SetValue(ItemsProperty, value);
                this.DataContext = new RandomViewModel() { Files = value };
            }
        }
        public FilesScreen()
        {
            InitializeComponent();
            DataContext = new RandomViewModel()
            {
                Files = this.Items
            };
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
