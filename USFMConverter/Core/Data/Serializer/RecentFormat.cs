using Newtonsoft.Json;

namespace USFMConverter.Core.Data.Serializer
{
    /// <summary>
    /// Json serializer class
    /// </summary>
    public class RecentFormat
    {
        public int FormatIndex { get; set; }
        [JsonProperty(nameof(FormatName))]
        public string FormatName { get; set; }
    }
}
