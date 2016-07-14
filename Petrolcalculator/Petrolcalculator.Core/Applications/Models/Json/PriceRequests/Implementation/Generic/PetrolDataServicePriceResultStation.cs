using Newtonsoft.Json;

namespace Petrolcalculator.Core.Applications.Models.Json.PriceRequests.Implementation.Generic
{
    /// <summary>
    /// Generic Data Model for Stations
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataServicePriceResultStation
    {
        /// <summary>
        /// Second Level of JSON
        /// Station with their IDs
        /// </summary>
        [JsonProperty("005056ba-7cb6-1ed2-bceb-b1139ea28d45")]
        public PetrolDataServicePriceResultModel Station { get; set; }

        // Now for the generic way every new Station with its ID has to be added as a single property with the correct JsonProperty to map
        // ....
    }
}
