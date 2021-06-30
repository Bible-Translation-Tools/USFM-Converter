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
using USFMConverter.UI.Pages.PartialView;
using System;
using System.Runtime.InteropServices;

namespace USFMConverter.UI.Pages
{
    public class FileView : UserControl
    {
        private ProjectReady projectReadySection;
        private ProjectNotReady projectNotReadySection;
        
        private Border dragDropArea;
        private ListBox filesContainer;
        private TextBlock selectedCount;

        public event EventHandler<RoutedEventArgs> ConvertStart
        {
            add
            {
                AddHandler(StartConvertEvent, value);
            }
            remove
            {
                RemoveHandler(StartConvertEvent, value);
            }
        }

        public static readonly RoutedEvent<RoutedEventArgs> StartConvertEvent =
            RoutedEvent.Register<FileView, RoutedEventArgs>(
                nameof(ConvertStart),
                RoutingStrategies.Direct
            );

        public FileView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            RenderLinuxDifference();

            projectReadySection = this.FindControl<ProjectReady>("ProjectReady");
            projectNotReadySection = this.FindControl<ProjectNotReady>("ProjectNotReady");         

            filesContainer = this.FindControl<ListBox>("FilesListBox");
            filesContainer.AddHandler(ListBox.SelectionChangedEvent, OnFileSelect);
            filesContainer.AddHandler(DragDrop.DragOverEvent, OnDragOver);
            filesContainer.AddHandler(DragDrop.DropEvent, OnDrop);

            dragDropArea = this.FindControl<Border>("DragDropArea");
            dragDropArea.AddHandler(DragDrop.DragOverEvent, OnDragOver);
            dragDropArea.AddHandler(DragDrop.DropEvent, OnDrop);

            selectedCount = this.FindControl<TextBlock>("SelectedCount");
        }

        private async void OnBrowseFolderClick(object? sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            var result = await dialog.ShowAsync((Window)this.VisualRoot);
            if (!string.IsNullOrEmpty(result))
            {
                var dir = new FileInfo(result);
                var filesInDir = FileSystem.GetFilesInDir(
                    dir, CoreConverter.supportedExtensions
                ).Select(f => f.FullName);

                var currentFileList = filesContainer.Items.Cast<string>();
                var newList = currentFileList.Concat(filesInDir).ToList(); ;

                filesContainer.Items = newList; // changes to the UI will bind to DataContext
                UpdateProjectStatus();
            }
        }

        private async void OnBrowseFilesClick(object? sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.AllowMultiple = true;
            dialog.Filters.Add(new FileDialogFilter
            {
                Name = "USFM Documents",
                Extensions = CoreConverter.supportedExtensions.ToList()
            });

            var paths = await dialog.ShowAsync((Window)this.VisualRoot);
            var currentFileList = filesContainer.Items.Cast<string>();
            var newList = currentFileList.Concat(paths).ToList(); ;

            filesContainer.Items = newList; // changes to the UI will bind to DataContext
            UpdateProjectStatus();
        }

        private void OnRemoveClick(object? sender, RoutedEventArgs e)
        {
            var list = filesContainer.Items.Cast<string>().ToList();
            
            foreach(var item in filesContainer.SelectedItems)
            {
                list.Remove(item.ToString());
            }

            filesContainer.Items = list;

            UpdateProjectStatus();
            UpdateCounter();
        }

        private void OnFileSelect(object? sender, SelectionChangedEventArgs e)
        {
            UpdateCounter();
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

                var currentFileList = filesContainer.Items.Cast<string>();
                var newList = currentFileList.Concat(filesToAdd).ToList();

                filesContainer.Items = newList; // changes to the UI will bind to DataContext
                UpdateProjectStatus();
            }
        }

        private void OnConvertStart(object? sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(StartConvertEvent));
        }

        /// <summary>
        /// Avalonia currently does not support drag and drop.
        /// Please remove this method once the framework implemented it.
        /// <see cref="https://github.com/AvaloniaUI/Avalonia/issues/5273"/>
        /// </summary>
        private void RenderLinuxDifference()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                TextBlock dndText = this.FindControl<TextBlock>("DragDropText");
                dndText.Text = "Browse for folder that contains USFM files";

                this.FindControl<Grid>("DragDropSection").Background = null;
                this.FindControl<Button>("BrowseFilesBtn").IsVisible = true;
            }
        }

        public void UpdateProjectStatus()
        {
            if (filesContainer.ItemCount > 0)
            {
                projectNotReadySection.IsVisible = false;
                projectReadySection.IsVisible = true;
            }
            else
            {
                projectNotReadySection.IsVisible = true;
                projectReadySection.IsVisible = false;
            }
        }

        public void UpdateCounter()
        {
            selectedCount.Text = filesContainer.SelectedItems.Count.ToString();
        }
    }
}