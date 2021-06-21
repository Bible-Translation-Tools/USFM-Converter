using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace USFMConverter.UI.Pages
{

    public partial class ProjectDetailScreen : UserControl
    {
        private Border dragDropArea;
        private ListBox filesContainer;

        private static readonly StyledProperty<List<string>> ItemsProperty = AvaloniaProperty.Register<ProjectDetailScreen, List<string>>(nameof(Items));
        public List<string> Items
        {
            get => GetValue(ItemsProperty);
            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        public ProjectDetailScreen()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            Items = new List<string>();
            filesContainer = this.Find<ListBox>("FilesListBox");

            dragDropArea = this.Find<Border>("DragDropArea");
            dragDropArea.AddHandler(DragDrop.DragOverEvent, OnDragOver);
            dragDropArea.AddHandler(DragDrop.DropEvent, OnDrop);
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                e.DragEffects = DragDropEffects.Copy;
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                var newList = Items.Concat(e.Data.GetFileNames()).ToList();
                Items = newList;

                //var viewData = (ViewData)DataContext;
                //viewData.Files = this.Items;

                filesContainer.Items = newList;
            }
        }

    }
}