using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using USFMConverter.UI;

namespace USFMConverter.UI.Pages
{
    public class OptionView : UserControl
    {
        private Button closeBtn;
        private TextBlock blurredArea;
        private UserControl optionView;

        private ComboBox outputOption;

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

            outputOption = this.Find<ComboBox>("OutputOption");
            outputOption.AddHandler(ComboBox.SelectionChangedEvent, OnOuputSelection);
            
            optionView = this.Find<UserControl>("OptionView");
        }

        private void OnOuputSelection(object? sender, SelectionChangedEventArgs e)
        {
            string selectedFormat;
            foreach (var comboBoxItem in outputOption.Items)
            {
                selectedFormat = ((ComboBoxItem)comboBoxItem).Tag.ToString();
                this.Find<UserControl>(selectedFormat).IsVisible = false;
            }
            
            selectedFormat = ((ComboBoxItem)outputOption.SelectedItem).Tag.ToString();
            this.Find<UserControl>(selectedFormat).IsVisible = true;
        }

        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            optionView.IsVisible = false;
        }
    }
}