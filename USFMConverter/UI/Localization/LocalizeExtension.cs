using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USFMConverter.UI.Localization
{
    public class LocalizeExtension : MarkupExtension
    {
        public LocalizeExtension()
        {

        }

        public string Key { get; set; }

        public string Context { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var keyToUse = Key;
            if (!string.IsNullOrWhiteSpace(Context))
                keyToUse = $"{Context}/{Key}";

            // this means Localizer.Instance[keyToUse] binding to xaml
            var binding = new ReflectionBindingExtension($"[{keyToUse}]")
            {
                Mode = BindingMode.Default,
                Source = Localizer.Instance,
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
