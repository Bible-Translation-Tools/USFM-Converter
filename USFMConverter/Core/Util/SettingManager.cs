using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using USFMConverter.Core.Data.Serializer;
using USFMConverter.UI;

namespace USFMConverter.Core.Util
{
    public static class SettingManager
    {
        private const string SETTING_FILE_TEMPLATE = "user_settings_{0}.json";
        private const string RECENT_FORMAT = "recent_format";
        private const string appName = "USFMConverter";
        private static string appDir;

        static SettingManager()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                string userHomeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                localAppData = Path.Combine(userHomeDir, "Library", "Application Support");
            }
            
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

        public static void SaveMostRecentFormat(RecentFormat recentFormat)
        {
            string path = Path.Combine(appDir, string.Format(SETTING_FILE_TEMPLATE, RECENT_FORMAT));
            string json = JsonConvert.SerializeObject(recentFormat, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static void SaveFormatSetting(string formatName, Setting setting)
        {
            string settingFile = string.Format(SETTING_FILE_TEMPLATE, formatName);
            string path = Path.Combine(appDir, settingFile);

            File.WriteAllText(path, JsonConvert.SerializeObject(setting, Formatting.Indented));
        }
    }
}