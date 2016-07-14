using System;
using Newtonsoft.Json;

namespace Petrolcalculator.Core.Applications.Models.Json.PriceRequests.Implementation
{
    /// <summary>
    /// Result Data Model for Petrol Data Service Requests
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    [Serializable]
    public class PetrolDataServicePriceResultModel
    {
        /// <summary>
        /// Status of a Petrol Station
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Price for e5
        /// </summary>
        [JsonProperty("e5")]
        public string E5 { get; set; }

        /// <summary>
        /// Price for e10
        /// </summary>
        [JsonProperty("e10")]
        public string E10 { get; set; }

        /// <summary>
        /// Price for diesel
        /// </summary>
        [JsonProperty("diesel")]
        public string Diesel { get; set; }
    }
}
