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

        static SettingManager()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            appDir = Path.Combine(localAppData, Assembly.GetExecutingAssembly().GetName().Name);
            Directory.CreateDirectory(appDir); // Create directory if doesn't exist. Ignore if it exists.

        }
        
        public static Setting? LoadOptionConfig(string OutputFileFormat)
        {
            string path = Path.Combine(appDir, String.Format(SETTING_FILE_TEMPLATE, OutputFileFormat));
            return File.Exists(path) ? JsonConvert.DeserializeObject<Setting>(File.ReadAllText(path)) : null;
        }

        public static void SaveOptionConfig(ViewData dataContext)
        {
            Setting setting = new (dataContext);
            string path = Path.Combine(appDir, String.Format(SETTING_FILE_TEMPLATE, dataContext.OutputFileFormat.Tag));

            // If file doesn't exist, create the file
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "{}");
            }
            
            File.WriteAllText(path, JsonConvert.SerializeObject(setting, Formatting.Indented));
        }

        public static string LoadLastUsedFormat()
        {
            string path = Path.Combine(appDir, String.Format(SETTING_FILE_TEMPLATE, "format"));
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

        public static void SaveLastUsedFormat(ViewData dataContext)
        {
            string path = Path.Combine(appDir, String.Format(SETTING_FILE_TEMPLATE, "format"));
            string lastUsedFormat = dataContext.OutputFileFormat.Tag.ToString();
            
            string jsonFile = File.ReadAllText(path);
            JObject jsonObj = JObject.Parse(jsonFile);

            jsonObj["LastUsedFormat"] = lastUsedFormat;
            
            File.WriteAllText(path, JsonConvert.SerializeObject(jsonObj, Formatting.Indented));
        }

        public static void SaveConfigs(ViewData dataContext)
        {
            SaveOptionConfig(dataContext);
            SaveLastUsedFormat(dataContext);
        }
    }
}