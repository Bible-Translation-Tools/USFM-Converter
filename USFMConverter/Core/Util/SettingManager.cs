using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using USFMConverter.Core.Data;
using USFMConverter.UI;

namespace USFMConverter.Core.Util
{
    public static class SettingManager
    {
        private static string appDir;
        private const string SETTING_FILE_TEMPLATE = "appsettings_{0}.json";
        private const string RECENT_FORMAT = "recent_format";

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
            catch { }

            return setting;
        }

        public static void SaveSetting(ViewData dataContext)
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

        public static int LoadMostRecentFormatIndex()
        {
            string path = Path.Combine(appDir, String.Format(SETTING_FILE_TEMPLATE, RECENT_FORMAT));
            int formatIndex = 0;

            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    var recentFormat = JsonConvert.DeserializeObject<RecentFormat>(json);

                    formatIndex = (recentFormat != null) ? recentFormat.FormatIndex : 0;
                }
                catch { }
            }

            return formatIndex;
        }

        public static string? LoadMostRecentFormat()
        {
            string path = Path.Combine(appDir, String.Format(SETTING_FILE_TEMPLATE, RECENT_FORMAT));
            string? format = null;

            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    var recentFormat = JsonConvert.DeserializeObject<RecentFormat>(json);

                    format = recentFormat?.FormatName;
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

            // If file doesn't exist, create the file
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "{}");
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(setting, Formatting.Indented));
        }

        /// <summary>
        /// Private json serializer class
        /// </summary>
        private class RecentFormat
        {
            public int FormatIndex { get; set; }
            public string FormatName { get; set; }
        }
    }
}