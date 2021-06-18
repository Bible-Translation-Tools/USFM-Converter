using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using Avalonia.Interactivity;

namespace USFMConverter.UI.Pages
{
  public partial class ProjectDetailScreen : UserControl
  {
    
    private static readonly StyledProperty<List<string>> ItemsProperty = AvaloniaProperty.Register<ProjectDetailScreen, List<string>>(nameof(Items));

    public List<string> Items
    {
      get => GetValue(ItemsProperty);
      set
      {
        SetValue(ItemsProperty, value);
        this.DataContext = new RandomViewModel() { Files = value };
      }
    }

    public ProjectDetailScreen()
    {
      InitializeComponent();
      DataContext = new RandomViewModel()
      {
        Files = this.Items
      };
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }

    public class RandomViewModel
    { 
      public List<string> Files { get; set; } = new() { "1", "2", "3" };
    }
  }
  
}