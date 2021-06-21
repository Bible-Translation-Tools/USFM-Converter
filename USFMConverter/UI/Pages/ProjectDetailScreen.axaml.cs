using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using USFMConverter.Core;
using USFMConverter.Core.Util;

namespace USFMConverter.UI.Pages
{

    public partial class ProjectDetailScreen : UserControl
    {
        private Border dragDropArea;
        private ListBox filesContainer;

        private static readonly StyledProperty<List<string>> ItemsProperty = AvaloniaProperty.Register<ProjectDetailScreen, List<string>>(nameof(Items));
        public List<string> Items
        {
            get => GetValue(ItemsProperty);
            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        public ProjectDetailScreen()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            SetLinuxText();

            Items = new List<string>();
            filesContainer = this.Find<ListBox>("FilesListBox");

            dragDropArea = this.Find<Border>("DragDropArea");
            dragDropArea.AddHandler(DragDrop.DragOverEvent, OnDragOver);
            dragDropArea.AddHandler(DragDrop.DropEvent, OnDrop);
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                e.DragEffects = DragDropEffects.Copy;
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                var filesToAdd = new List<string>();
                var selectedFiles = e.Data.GetFileNames()
                        .Select(name => new FileInfo(name));

                foreach (var file in selectedFiles)
                {
                    if (file.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        var filesInDir = FileSystem.GetFilesInDir(
                            file, CoreConverter.supportedExtensions
                        ).Select(f => f.FullName);

                        filesToAdd.AddRange(filesInDir);
                    }
                    else if (CoreConverter.supportedExtensions.Contains(file.Extension))
                    {
                        filesToAdd.Add(file.FullName);
                    }
                }

                var newList = Items.Concat(filesToAdd).ToList();
                Items = newList;

                filesContainer.Items = newList; // changes to the UI will bind to DataContext
            }
        }

        private void SetLinuxText()
        {
            var platform = AvaloniaLocator.Current.GetService<IRuntimePlatform>().GetRuntimeInfo().OperatingSystem;
            if (platform == OperatingSystemType.Linux)
            {
                TextBlock dndText = this.Find<TextBlock>("DragDropText");
                dndText.Text = "Browse for folder that contains .usfm files";
            }
        }
    }
}