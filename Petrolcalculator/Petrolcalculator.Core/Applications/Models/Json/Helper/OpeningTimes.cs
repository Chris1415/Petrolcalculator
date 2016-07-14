using Newtonsoft.Json;

namespace Petrolcalculator.Core.Applications.Models.Json.Helper
{
    /// <summary>
    /// Class to Hold the Information about opening Times
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class OpeningTimes
    {
        /// <summary>
        /// Text - Day
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Start Time
        /// </summary>
        [JsonProperty("start")]
        public string Start { get; set; }

        /// <summary>
        /// End Time
        /// </summary>
        [JsonProperty("end")]
        public string End { get; set; }
    }
}
