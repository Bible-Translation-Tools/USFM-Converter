using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace USFMConverter.UI.Pages
{
  public partial class ProjectDetailScreen : UserControl
  {
    public ProjectDetailScreen()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}