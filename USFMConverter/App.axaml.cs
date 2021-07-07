using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using USFMConverter.Core.Data;
using USFMConverter.Core.Util;
using USFMConverter.UI;

namespace USFMConverter
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Setting? setting = FileSystem.LoadConfig();

                // If there exist config files, load them and assign to the data context
                if (setting != null)
                {
                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = new ViewData()
                        {
                            SelectedTextSizeIndex = setting.TextSize,
                            SelectedLineSpacingIndex = setting.LineSpacing,
                            ColumnCount = setting.ColumnCount,
                            Justified = setting.Justified,
                            LeftToRight = setting.LeftToRight,
                            ChapterBreak = setting.ChapterBreak,
                            VerseBreak = setting.VerseBreak,
                            NoteTaking = setting.NoteTaking,
                            TableOfContents = setting.TableOfContents
                        }
                    };
                }
                // If the config file does not exist
                else
                {
                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = new ViewData()
                    };
                }
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}