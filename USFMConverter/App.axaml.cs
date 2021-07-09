using System;
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
                string? recentFormat = SettingManager.LoadMostRecentFormat();
                if (!string.IsNullOrEmpty(recentFormat))
                {
                    Setting? setting = SettingManager.LoadSetting(recentFormat);
                    // Load if there is saved setting
                    if (setting != null)
                    {
                        desktop.MainWindow = new MainWindow
                        {
                            DataContext = new ViewData
                            {
                                SelectedTextSizeIndex = setting.TextSizeIndex,
                                SelectedLineSpacingIndex = setting.LineSpacingIndex,
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
                }
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