using System.Collections.Generic;
using Newtonsoft.Json;
using Petrolcalculator.Core.Applications.Models.Json.Helper;
using Petrolcalculator.Core.Applications.Models.Json.ListRequests.Implementation;

namespace Petrolcalculator.Core.Applications.Models.Json.DetailRequests.Implementation
{
    /// <summary>
    /// Result Data Model for Petrol Data Service Requests
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataServiceDetailResultModel : PetrolDataServiceResultModel
    {
        /// <summary>
        /// Get Information if the Opeining Times are overriden
        /// </summary>
        [JsonProperty("overrides")]
        public string Overrides { get; set; }

        /// <summary>
        /// Flag to determine if the Petrol Station is opened the whole Day
        /// </summary>
        [JsonProperty("wholeDay")]
        public string WholeDay { get; set; }

        /// <summary>
        /// State 
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// The Opening Times
        /// </summary>
        [JsonProperty("openingTimes")]
        public List<OpeningTimes> OpeningTimes { get; set; }
    }
}
