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

            return File.Exists(path) 
                ? JsonConvert.DeserializeObject<Setting>(File.ReadAllText(path)) 
                : null;
        }

        public static void SaveSetting(ViewData dataContext)
        {
            string formatName = dataContext.OutputFileFormat.Tag.ToString();
            SaveMostRecentFormat(formatName);

            var setting = new Setting(dataContext);
            SaveFormatSetting(formatName, setting);
        }

        public static string LoadMostRecentFormat()
        {
            string path = Path.Combine(appDir, String.Format(SETTING_FILE_TEMPLATE, RECENT_FORMAT));
            string lastUsedFormat = "";

            if (!File.Exists(path))
            {
                string content = "{\"LastUsedFormat\": \"\"}";
                File.WriteAllText(path, content);
            }
            else
            {
                string jsonFile = File.ReadAllText(path);
                JObject jsonObj = JObject.Parse(jsonFile);

                lastUsedFormat = (string) jsonObj["LastUsedFormat"];
            }

            return lastUsedFormat;
        }

        private static void SaveMostRecentFormat(string formatName)
        {
            string path = Path.Combine(appDir, String.Format(SETTING_FILE_TEMPLATE, RECENT_FORMAT));
            
            string jsonFile = File.ReadAllText(path);
            JObject jsonObj = JObject.Parse(jsonFile);

            jsonObj["LastUsedFormat"] = formatName;
            
            File.WriteAllText(path, JsonConvert.SerializeObject(jsonObj, Formatting.Indented));
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
    }
}