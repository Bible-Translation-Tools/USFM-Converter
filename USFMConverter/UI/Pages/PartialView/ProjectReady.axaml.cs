using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using USFMConverter.Core.ConstantValue;

namespace USFMConverter.UI.Pages.PartialView
{
    public class ProjectReady : UserControl
    {
        private Dictionary<FileFormat, FileDialogFilter> fileFilter = new()
        {
            [FileFormat.DOCX] = new FileDialogFilter
            {
                Name = "Word Document",
                Extensions = new() { "docx" }
            },
            [FileFormat.HTML] = new FileDialogFilter
            {
                Name = "HTML Document",
                Extensions = new() { "html" }
            }
        };

        public ProjectReady()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OnConvertClick(object? sender, RoutedEventArgs e)
        {
            var fileFormatName = ((ViewData)DataContext).OutputFileFormat.Tag?.ToString();

            if (string.IsNullOrEmpty(fileFormatName))
            {
                return; // invalid output format
            }

            FileFormat fileFormat = Enum.Parse<FileFormat>(fileFormatName);

            var dialog = new SaveFileDialog();
            dialog.Title = "Save Project As";
            dialog.InitialFileName = "out";
            dialog.Filters.Add(fileFilter[fileFormat]);

            var result = await dialog.ShowAsync((Window)this.VisualRoot);
            if (!string.IsNullOrEmpty(result))
            {
                // propagate event to parent
                ((ViewData)DataContext).OutputFileLocation = result;
                RaiseEvent(new RoutedEventArgs(ProjectDetailScreen.StartConvertEvent));
            }
        }
    }
}