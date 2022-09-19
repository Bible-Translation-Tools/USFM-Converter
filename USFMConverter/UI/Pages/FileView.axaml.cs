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
using USFMConverter.Core.Data;

namespace USFMConverter.UI.Pages
{
    public class FileView : UserControl
    {
        private ProjectReady projectReadySection;
        private ProjectNotReady projectNotReadySection;

        private Border dragDropArea;
        private ListBox filesContainer;
        private TextBlock selectedCount;
        private Button selectAllBtn;
        private Button moveUpBtn;
        private Button moveDownBtn;
        private bool selected = false;

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

        public event EventHandler<RoutedEventArgs> BrowseError
        {
            add
            {
                AddHandler(BrowseErrorEvent, value);
            }
            remove
            {
                RemoveHandler(BrowseErrorEvent, value);
            }
        }

        public static readonly RoutedEvent<RoutedEventArgs> StartConvertEvent =
            RoutedEvent.Register<FileView, RoutedEventArgs>(
                nameof(ConvertStart),
                RoutingStrategies.Direct
            );

        public static readonly RoutedEvent<RoutedEventArgs> BrowseErrorEvent =
            RoutedEvent.Register<FileView, RoutedEventArgs>(
                nameof(BrowseError),
                RoutingStrategies.Bubble
            );


        public FileView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            RenderLinuxUI();

            projectReadySection = this.FindControl<ProjectReady>("ProjectReady");
            projectNotReadySection = this.FindControl<ProjectNotReady>("ProjectNotReady");

            filesContainer = this.FindControl<ListBox>("FilesListBox");
            filesContainer.AddHandler(DragDrop.DragOverEvent, OnDragOver);
            filesContainer.AddHandler(DragDrop.DropEvent, OnDrop);

            dragDropArea = this.FindControl<Border>("DragDropArea");
            dragDropArea.AddHandler(DragDrop.DragOverEvent, OnDragOver);
            dragDropArea.AddHandler(DragDrop.DropEvent, OnDrop);

            selectedCount = this.FindControl<TextBlock>("SelectedCount");
            selectAllBtn = this.FindControl<Button>("SelectAllBtn");
            moveUpBtn = this.FindControl<Button>("MoveUpBtn");
            moveDownBtn = this.FindControl<Button>("MoveDownBtn");
        }

        private async void OnBrowseFolderClick(object? sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            dialog.Title = "Select a Folder";

            var result = await dialog.ShowAsync((Window)this.VisualRoot);

            if (string.IsNullOrEmpty(result))
            {
                return;
            }
            
            var dir = new FileInfo(result);
            List<IProjectItem> filesInDir = new List<IProjectItem>();
            try
            {
                filesInDir.AddRange(FileSystem.GetFilesInDir(
                    dir, CoreConverter.supportedExtensions
                ).Select(  f => new FileItem(f.FullName)));
                foreach (var file in Directory.EnumerateFiles(result, "manifest.json", SearchOption.AllDirectories))
                {
                    filesInDir.Add(new WriterProjectItem() { Path = file });
                }
            }
            catch (Exception ex)
            {
                ((ViewData)DataContext).Error = ex;
                RaiseEvent(new RoutedEventArgs(BrowseErrorEvent));
                return;
            }
                
            var currentFileList = filesContainer.Items.Cast<IProjectItem>();
            var newList = currentFileList.Concat(filesInDir).ToList();

            filesContainer.Items = newList; // changes to the UI will bind to DataContext
            UpdateProjectStatus();
        }


        private void OnSelectAllClick(object? sender, RoutedEventArgs e)
        {
            if (selected)
            {
                // unselect
                filesContainer.SelectedIndex = -1;
                selectAllBtn.Content = "Select All";
                selected = false;
            }
            else
            {
                filesContainer.SelectAll();
                selectAllBtn.Content = "Unselect All";
                selected = true;
            }
        }

        private async void OnBrowseFilesClick(object? sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.AllowMultiple = true;
            dialog.Title = "Select Files";

            var extensions = CoreConverter.supportedExtensions
                .Select(ex => ex.Replace(".", ""))
                .ToList();

            dialog.Filters.Add(new FileDialogFilter
            {
                Name = "USFM Documents",
                Extensions = extensions
            });

            var paths = await dialog.ShowAsync((Window)this.VisualRoot);
            if (paths == null || paths.Length == 0) return;

            var currentFileList = filesContainer.Items.Cast<IProjectItem>();
            var newList = currentFileList.Concat(paths.Select(i => new FileItem(i))).ToList(); ;

            filesContainer.Items = newList; // changes to the UI will bind to DataContext
            UpdateProjectStatus();
        }

        private void OnRemoveClick(object? sender, RoutedEventArgs e)
        {
            var list = filesContainer.Items.Cast<IProjectItem>().ToList();

            foreach (var item in filesContainer.SelectedItems)
            {
                list.Remove(item as IProjectItem);
            }

            filesContainer.Items = list;

            UpdateProjectStatus();
            UpdateCounter();
            UpdateSelectBtn();
        }

        private void OnListItemSelect(object? sender, SelectionChangedEventArgs e)
        {
            UpdateCounter();
            UpdateSelectBtn();
            UpdateMoveBtn();
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
                var filesToAdd = new List<IProjectItem>();
                var selectedFiles = e.Data.GetFileNames()
                        .Select(name => new FileInfo(name));

                foreach (var file in selectedFiles)
                {
                    if (file.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        try
                        {
                            var filesInDir = FileSystem.GetFilesInDir(
                                file, CoreConverter.supportedExtensions
                            ).Select(f => new FileItem(f.FullName));

                            filesToAdd.AddRange(filesInDir);
                        } 
                        catch (Exception ex)
                        {
                            // some folder may not have read permission
                            ((ViewData)DataContext).Error = ex;
                            RaiseEvent(new RoutedEventArgs(BrowseErrorEvent));
                            return;
                        }
                    }
                    else if (CoreConverter.supportedExtensions.Contains(file.Extension))
                    {
                        filesToAdd.Add(new FileItem(file.FullName));
                    }
                }

                var currentFileList = filesContainer.Items.Cast<IProjectItem>();
                var newList = currentFileList.Concat(filesToAdd).ToList();

                filesContainer.Items = newList; // changes to the UI will bind to DataContext
                UpdateProjectStatus();
            }
        }

        private void OnMoveUp(object? sender, RoutedEventArgs e)
        {
            var index = filesContainer.SelectedIndex;
            if (index <= 0) return;

            var selectedFile = filesContainer.SelectedItem as IProjectItem;
            var list = filesContainer.Items.Cast<IProjectItem>().ToList();
            list.RemoveAt(index);
            list.Insert(--index, selectedFile);
            filesContainer.Items = list;
            filesContainer.SelectedItem = list[index]; // untoggle others
            filesContainer.SelectedIndex = index;
        }

        private void OnMoveDown(object? sender, RoutedEventArgs e)
        {
            var index = filesContainer.SelectedIndex;
            if (index < 0 || index >= (filesContainer.ItemCount - 1))
            {
                return;
            }

            var selectedFile = filesContainer.SelectedItem as IProjectItem;
            var list = filesContainer.Items.Cast<IProjectItem>().ToList();
            list.RemoveAt(index);
            list.Insert(++index, selectedFile);
            filesContainer.Items = list;
            filesContainer.SelectedItem = list[index]; // untoggle others
            filesContainer.SelectedIndex = index;
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
        private void RenderLinuxUI()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                TextBlock dndText = this.FindControl<TextBlock>("DragDropText");
                dndText.Text = "Browse for folder that contains USFM files";

                this.FindControl<Image>("DragDropImage").IsVisible = false;
            }
        }

        public void UpdateProjectStatus()
        {
            bool isReady = (filesContainer.ItemCount > 0);
            projectReadySection.IsVisible = isReady;
            projectNotReadySection.IsVisible = !isReady;
        }

        public void UpdateCounter()
        {
            selectedCount.Text = filesContainer.SelectedItems.Count.ToString();
        }

        public void UpdateSelectBtn()
        {
            bool anySelected = (filesContainer.SelectedItems.Count > 0);
            selectAllBtn.Content = anySelected ? "Unselect All" : "Select All";
            selected = anySelected;
        }

        private void UpdateMoveBtn()
        {
            if (filesContainer.SelectedItems.Count == 1)
            {
                moveUpBtn.IsEnabled = true;
                moveDownBtn.IsEnabled = true;
            }
            else
            {
                moveUpBtn.IsEnabled = false;
                moveDownBtn.IsEnabled = false;
            }
        }

        private void OnSortFiles(object sender, RoutedEventArgs e)
        {
            var files = filesContainer.Items.Cast<IProjectItem>().ToList();
            filesContainer.Items = files.OrderBy(f => Path.GetFileName(f.Label)).ToList();
        }
    }
}