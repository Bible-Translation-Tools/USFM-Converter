using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace USFMConverter.UI.Pages
{
    public class OptionView : UserControl
    {
        private Button closeBtn;
        private UserControl optionView;

        public OptionView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            closeBtn = this.Find<Button>("CloseBtn");
            closeBtn.AddHandler(Button.ClickEvent, OnCloseClick);

            optionView = this.Find<UserControl>("OptionView");
        }

        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            optionView.IsVisible = false;
        }
    }
}