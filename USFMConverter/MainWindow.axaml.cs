using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.IO;
using USFMConverter.Pages;

namespace USFMConverter
{
    public partial class MainWindow : Window
    {
        HomeScreen HomeScreen;
        FilesScreen filesScreen;
        Dictionary<string, IControl> Screens = new Dictionary<string, IControl>();
        public MainWindow()
        {
            InitializeComponent();
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
        public void HomeScreen_FolderSelected(object sender, RoutedEventArgs e)
        {
            HomeScreen HomeScreen = (HomeScreen)sender;
            filesScreen.Items = LoadFolder(HomeScreen.Folder);
            SetCurrentScreen("FilesScreen");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.HomeScreen = this.FindControl<HomeScreen>("HomeScreen");
            this.filesScreen = this.FindControl<FilesScreen>("FilesScreen");
            this.Screens.Add("HomeScreen", this.HomeScreen);
            this.Screens.Add("FilesScreen", this.filesScreen);
        }

        private List<string> LoadFolder(string folderName)
        {
            List<string> output = new();
            List<string> supportedExtensions = new List<string> { ".usfm", ".txt", ".sfm" };
            var dirinfo = new DirectoryInfo(folderName);
            var allFiles = dirinfo.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo fileInfo in allFiles)
            {
                if (supportedExtensions.Contains(Path.GetExtension(fileInfo.FullName.ToLower())))
                {
                    output.Add(fileInfo.FullName);
                }
            }
            return output;
        }
    }
}
