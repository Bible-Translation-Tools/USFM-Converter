using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using USFMConverter.Core.Data.Serializer;
using USFMConverter.UI;

namespace USFMConverter.Core.Util
{
    public static class SettingManager
    {
        private const string SETTING_FILE_TEMPLATE = "appsettings_{0}.json";
        private const string RECENT_FORMAT = "recent_format";

        private static string appDir;

        static SettingManager()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appName = Assembly.GetExecutingAssembly().GetName().Name
                ?? "USFMConverter";

            appDir = Path.Combine(localAppData, appName);
            Directory.CreateDirectory(appDir); // creates directory if doesn't exist; otherwise does nothing
        }

        public static Setting? LoadSetting(string outputFileFormat)
        {
            string path = Path.Combine(appDir, string.Format(SETTING_FILE_TEMPLATE, outputFileFormat));
            Setting? setting = null;
            
            try
            {
                setting = File.Exists(path)
                    ? JsonConvert.DeserializeObject<Setting>(File.ReadAllText(path))
                    : null;
            }
            catch { } // skip previous settings if error

            return setting;
        }

        public static void SaveSettings(ViewData dataContext)
        {
            var setting = new Setting(dataContext);
            var formatName = dataContext.OutputFileFormat.Tag.ToString();
            var recentFormat = new RecentFormat
            {
                FormatIndex = dataContext.SelectedFormatIndex,
                FormatName = formatName
            };

            SaveMostRecentFormat(recentFormat);
            SaveFormatSetting(formatName, setting);
        }

        public static RecentFormat? LoadMostRecentFormat()
        {
            string path = Path.Combine(appDir, String.Format(SETTING_FILE_TEMPLATE, RECENT_FORMAT));
            RecentFormat? format = null;

            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    format = JsonConvert.DeserializeObject<RecentFormat>(json);
                }
                catch { }
            }

            return format;
        }

        private static void SaveMostRecentFormat(RecentFormat recentFormat)
        {
            string path = Path.Combine(appDir, string.Format(SETTING_FILE_TEMPLATE, RECENT_FORMAT));
            string json = JsonConvert.SerializeObject(recentFormat, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        private static void SaveFormatSetting(string formatName, Setting setting)
        {
            string settingFile = string.Format(SETTING_FILE_TEMPLATE, formatName);
            string path = Path.Combine(appDir, settingFile);

            File.WriteAllText(path, JsonConvert.SerializeObject(setting, Formatting.Indented));
        }
    }
}