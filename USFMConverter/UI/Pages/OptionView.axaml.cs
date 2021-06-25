using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using USFMConverter.Core.ConstantValue;
using USFMConverter.UI;

namespace USFMConverter.UI.Pages
{
    public class OptionView : UserControl
    {
        private Button closeBtn;
        private TextBlock blurredArea;
        private UserControl optionView;
        private ComboBox outputFormatCb;

        public OptionView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            closeBtn = this.Find<Button>("CloseBtn");
            closeBtn.AddHandler(Button.ClickEvent, OnCloseClick);

            blurredArea = this.Find<TextBlock>("BlurredArea");
            blurredArea.AddHandler(PointerPressedEvent, OnCloseClick);

            outputFormatCb = this.Find<ComboBox>("OutputOption");
            outputFormatCb.AddHandler(ComboBox.SelectionChangedEvent, OnOuputFormatSelect);

            optionView = this.Find<UserControl>("OptionView");
        }

        private void OnOuputFormatSelect(object? sender, SelectionChangedEventArgs e)
        {
            string selectedFormat = ((ComboBoxItem)outputFormatCb.SelectedItem).Tag.ToString();
            
            foreach (ComboBoxItem comboBoxItem in outputFormatCb.Items)
            {
                var formatName = comboBoxItem.Tag.ToString();
                this.Find<UserControl>(formatName).IsVisible = (formatName == selectedFormat);
            }
        }

        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            optionView.IsVisible = false;
        }
    }
}