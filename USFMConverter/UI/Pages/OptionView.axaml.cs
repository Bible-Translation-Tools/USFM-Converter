using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace USFMConverter.UI.Pages
{
  public class OptionView : UserControl
  {
    public OptionView()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}