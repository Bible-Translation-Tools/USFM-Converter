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
        HomeScreen HomeScreen;
        FilesScreen FilesScreen;
        FormatScreen FormatScreen;
        ProjectDetailScreen ProjectDetailScreen;
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
            //FilesScreen.Items = LoadFolder(HomeScreen.Folder);
            //SetCurrentScreen("FilesScreen");
            SetCurrentScreen("ProjectDetailScreen");
        }

        public void FileScreen_FormatPage(object sender, RoutedEventArgs e)
        {
            FilesScreen filesScreen = (FilesScreen) sender;
            SetCurrentScreen("FormatScreen");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.HomeScreen = this.FindControl<HomeScreen>("HomeScreen");
            this.FilesScreen = this.FindControl<FilesScreen>("FilesScreen");
            this.FormatScreen = this.FindControl<FormatScreen>("FormatScreen");
            this.ProjectDetailScreen = this.FindControl<ProjectDetailScreen>("ProjectDetailScreen");
            this.Screens.Add("HomeScreen", this.HomeScreen);
            this.Screens.Add("FilesScreen", this.FilesScreen);
            this.Screens.Add("FormatScreen", this.FormatScreen);
            this.Screens.Add("ProjectDetailScreen", this.ProjectDetailScreen);
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
