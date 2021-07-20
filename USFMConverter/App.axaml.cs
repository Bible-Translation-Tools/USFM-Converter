using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using USFMConverter.Core.Data.Serializer;
using USFMConverter.Core.Util;
using USFMConverter.UI;
using USFMConverter.UI.Localization;

namespace USFMConverter
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            LoadLocalization();
        }

        private void LoadLocalization()
        {
            var language = CultureInfo.CurrentUICulture.Name.Split('-')[0]; // name format is <languagecode>-<region>

            // default to English if not available
            if (!Localizer.Instance.LoadLanguage(language))
            {
                Localizer.Instance.LoadLanguage();
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var recentFormat = SettingManager.LoadMostRecentFormat();
                if (recentFormat != null)
                {
                    Setting? setting = SettingManager.LoadSetting(recentFormat.FormatName);
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
                
                if (desktop.MainWindow == null)
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