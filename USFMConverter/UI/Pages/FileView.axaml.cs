using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Interactivity;
using USFMConverter.Core;
using USFMConverter.Core.Util;

namespace USFMConverter.UI.Pages
{
    public class FileView : UserControl
    {
        private Border dragDropArea;
        private ListBox filesContainer;
        private TextBlock selectedCount;
        private Button browseBtn;
        private Button removeFileBtn;

        private static readonly StyledProperty<List<string>> ItemsProperty = AvaloniaProperty.Register<ProjectDetailScreen, List<string>>(nameof(Items));
        
        public List<string> Items
        {
            get => GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public FileView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            SetLinuxText();

            Items = new List<string>();

            browseBtn = this.Find<Button>("BrowseBtn");
            browseBtn.AddHandler(Button.ClickEvent, OnBrowseClick);

            removeFileBtn = this.Find<Button>("RemoveFileBtn");
            removeFileBtn.AddHandler(Button.ClickEvent, OnRemoveClick);

            filesContainer = this.Find<ListBox>("FilesListBox");
            filesContainer.AddHandler(ListBox.SelectionChangedEvent, OnFileSelect);
            filesContainer.AddHandler(DragDrop.DragOverEvent, OnDragOver);
            filesContainer.AddHandler(DragDrop.DropEvent, OnDrop);

            dragDropArea = this.Find<Border>("DragDropArea");
            dragDropArea.AddHandler(DragDrop.DragOverEvent, OnDragOver);
            dragDropArea.AddHandler(DragDrop.DropEvent, OnDrop);

            selectedCount = this.Find<TextBlock>("SelectedCount");
        }

        private async void OnBrowseClick(object? sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            var result = await dialog.ShowAsync((Window)this.VisualRoot);
            if (!string.IsNullOrEmpty(result))
            {
                var dir = new FileInfo(result);
                var filesInDir = FileSystem.GetFilesInDir(
                    dir, CoreConverter.supportedExtensions
                ).Select(f => f.FullName);
                
                var newList = Items.Concat(filesInDir).ToList();
                Items = newList;

                filesContainer.Items = newList; // changes to the UI will bind to DataContext
            }
        }

        private void OnRemoveClick(object? sender, RoutedEventArgs e)
        {
            var list = filesContainer.Items.Cast<string>().ToList();
            
            foreach(var item in filesContainer.SelectedItems)
            {
                list.Remove(item.ToString());
            }

            Items = list;
            filesContainer.Items = list;

            UpdateCount();
        }

        private void OnFileSelect(object? sender, SelectionChangedEventArgs e)
        {
            UpdateCount();
        }

        private void OnDragOver(object? sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                e.DragEffects = DragDropEffects.Copy;
            }
        }

        private void OnDrop(object? sender, DragEventArgs e)
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

        private void UpdateCount()
        {
            selectedCount.Text = filesContainer.SelectedItems.Count + " Selected";
        }
    }
}