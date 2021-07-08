using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using USFMConverter.Core;

namespace USFMConverter.UI.Pages
{
    public class OptionView : UserControl
    {
        private Border blurredArea;
        private UserControl optionView;
        private ComboBox outputFormatCb;
        private ComboBox licenseSelectCb;

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

            licenseSelectCb = this.FindControl<ComboBox>("LicenseSelector");
            LoadLicenseOptions();

            optionView = this.FindControl<UserControl>("OptionView");
        }

        private void LoadLicenseOptions()
        {
            //var cbItem = new ComboBoxItem
            //{
            //    Tag = "en",
            //    Content = "English"
            //};
            //licenseSelectCb.Items = new List<ComboBoxItem> { cbItem };
            //licenseSelectCb.SelectedIndex = 0;
        }

        private async void OnCustomLicenseSelect(object? sender, PointerPressedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.AllowMultiple = false;
            dialog.Title = "Select a License file";
            dialog.Filters.Add(new FileDialogFilter
            {
                Name = "USFM Document",
                Extensions = new List<string> { "usfm" }
            });

            var files = await dialog.ShowAsync((Window)this.VisualRoot);
            if (
                files.Length == 0 || string.IsNullOrEmpty(files[0])
                )
            {
                return;
            }

            var file = new FileInfo(files[0]);
            ((ViewData)DataContext).LicenseFile = file.FullName;

            var items = licenseSelectCb.Items.Cast<ComboBoxItem>().ToList();
            items.Insert(0, new ComboBoxItem { 
                Content = file.Name,
                Tag = CoreConverter.CUSTOM_LICENSE
            });

            licenseSelectCb.Items = items;
            licenseSelectCb.SelectedIndex = 0;
        }

        private void OnOuputFormatSelect(object? sender, SelectionChangedEventArgs e)
        {
            string selectedFormat = ((ComboBoxItem)outputFormatCb.SelectedItem).Tag.ToString();
            
            foreach (ComboBoxItem comboBoxItem in outputFormatCb.Items)
            {
                var formatName = comboBoxItem.Tag.ToString();
                this.FindControl<UserControl>(formatName).IsVisible = (formatName == selectedFormat);
            }

            ResetInputOptions();
        }

        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            optionView.IsVisible = false;
        }

        private void ResetInputOptions()
        {
            var data = ((ViewData)DataContext);

            // change the DataContext of root element
            // will apply to all children
            ((Window)this.VisualRoot).DataContext = new ViewData
            {
                LicenseFile = data.LicenseFile,
                SelectedLicense = data.SelectedLicense,
                Files = data.Files
            };
        }
    }
}