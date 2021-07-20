using Avalonia;
using Avalonia.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USFMConverter.UI.Localization
{
    public class Localizer : INotifyPropertyChanged
    {
        private const string IndexerName = "Item";
        private const string IndexerArrayName = "Item[]";
        private Dictionary<string, string> i18nMap = null;

        public Localizer()
        {

        }

        public bool LoadLanguage(string language = "en")
        {
            Language = language;
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            Uri uri = new Uri($"avares://USFMConverter/UI/Assets/i18n/{language}.json");

            bool isLoaded = false;
            if (assets.Exists(uri))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(assets.Open(uri), Encoding.UTF8))
                    {
                        i18nMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
                    }
                    Invalidate();
                    isLoaded = true;
                } 
                catch
                {
                    isLoaded = false;
                }

            }

            return isLoaded;
        }

        public string Language { get; private set; }

        public string this[string key]
        {
            get
            {
                string res;
                if (i18nMap != null && i18nMap.TryGetValue(key, out res))
                    return res.Replace("\\n", "\n");

                return $"{Language}:{key}";
            }
        }

        public static Localizer Instance { get; set; } = new Localizer();
        public event PropertyChangedEventHandler PropertyChanged;

        // new text will be applied as soon as language is loaded
        public void Invalidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerArrayName));
        }
    }
}
