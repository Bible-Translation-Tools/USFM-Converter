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
            },
            [FileFormat.USFM] = new FileDialogFilter
            {
                Name = "USFM Document",
                Extensions = new() { "usfm" }
            }
        };

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

            var fileFormat = Enum.Parse<FileFormat>(fileFormatName);

            var context = ((ViewData)DataContext);
            string result;
            if (context.IndividualFiles)
            {
                var dialog = new OpenFolderDialog()
                {
                    Title = "Save Project Where"
                };
                
                result = await dialog.ShowAsync((Window)this.VisualRoot);
            }
            else
            {
                var dialog = new SaveFileDialog();
                dialog.Title = "Save Project As";
                dialog.InitialFileName = "out";
                dialog.Filters.Add(fileFilter[fileFormat]);

                result = await dialog.ShowAsync((Window)this.VisualRoot);
            }
            if (!string.IsNullOrEmpty(result))
            {
                // propagate event to parent
                context.OutputPath = result;
                RaiseEvent(new RoutedEventArgs(StartConvertEvent));
            }

            this.Focus(); // remove focus from Convert button
        }
    }
}