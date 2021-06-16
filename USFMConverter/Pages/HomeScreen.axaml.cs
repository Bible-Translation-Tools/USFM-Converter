using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace USFMConverter.Pages
{
    public partial class HomeScreen : UserControl
    {
        private string _folder = "";
        public string Folder => _folder;

        public HomeScreen()
        {
            InitializeComponent();
        }

        public static readonly RoutedEvent<RoutedEventArgs> FolderSelectedEvent = RoutedEvent.Register<HomeScreen, RoutedEventArgs>(nameof(FolderSelected), RoutingStrategies.Bubble);

        public event EventHandler<RoutedEventArgs> FolderSelected
        {
            add
            {
                AddHandler(FolderSelectedEvent, value);
            }
            remove
            {

                RemoveHandler(FolderSelectedEvent, value);
            }
        }

        public async void BrowseFiles_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            var result = await dialog.ShowAsync((Window)this.VisualRoot);
            if (!string.IsNullOrEmpty(result))
            {
                _folder = result;
                OnFolderSelected();
            }
        }

        private void OnFolderSelected()
        {
            var e = new RoutedEventArgs(FolderSelectedEvent);
            RaiseEvent(e);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
