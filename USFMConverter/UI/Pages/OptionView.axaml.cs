using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using USFMConverter.Core.Data;
using USFMConverter.Core.Util;

namespace USFMConverter.UI.Pages
{
    public class OptionView : UserControl
    {
        private Border blurredArea;
        private UserControl optionView;
        private ComboBox outputFormatCb;

        public OptionView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            blurredArea = this.FindControl<Border>("BlurredArea");
            blurredArea.AddHandler(PointerPressedEvent, OnCloseClick);

            outputFormatCb = this.FindControl<ComboBox>("OutputFormatSelector");
            outputFormatCb.AddHandler(ComboBox.SelectionChangedEvent, OnOuputFormatSelect);

            optionView = this.FindControl<UserControl>("OptionView");
        }

        private void OnOuputFormatSelect(object? sender, SelectionChangedEventArgs e)
        {
            string selectedFormat = ((ComboBoxItem) outputFormatCb.SelectedItem).Tag.ToString();

            foreach (ComboBoxItem comboBoxItem in outputFormatCb.Items)
            {
                var formatName = comboBoxItem.Tag.ToString();
                this.FindControl<UserControl>(formatName).IsVisible = (formatName == selectedFormat);
            }

            Console.WriteLine(selectedFormat);
            
            SaveOptions();
            LoadOptions(selectedFormat);
        }

        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            optionView.IsVisible = false;
            SaveOptions();
        }

        private void SaveOptions()
        {
            // Save config files when option view is closed
            var dataContext = (ViewData) DataContext;
            FileSystem.SaveConfig(dataContext);
        }

        private void LoadOptions(string OutputFileFormat)
        {
            var dataContext = (ViewData) DataContext;
            Setting? setting = FileSystem.LoadConfig(OutputFileFormat);

            ((Window) VisualRoot).DataContext = new ViewData
            {
                Files = dataContext.Files,
                SelectedTextSizeIndex = setting.TextSize,
                SelectedLineSpacingIndex = setting.LineSpacing,
                ColumnCount = setting.ColumnCount,
                Justified = setting.Justified,
                LeftToRight = setting.LeftToRight,
                ChapterBreak = setting.ChapterBreak,
                VerseBreak = setting.VerseBreak,
                NoteTaking = setting.NoteTaking,
                TableOfContents = setting.TableOfContents
            };
        }
    }
}