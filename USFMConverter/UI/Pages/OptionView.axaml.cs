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

        private UserControl docxOption;
        private UserControl htmlOption;
        
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
            
            optionView = this.Find<UserControl>("OptionView");
            
            docxOption = this.Find<UserControl>("DocxOption");
            htmlOption = this.Find<UserControl>("HtmlOption");
        }

        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            optionView.IsVisible = false;
        }
    }
}