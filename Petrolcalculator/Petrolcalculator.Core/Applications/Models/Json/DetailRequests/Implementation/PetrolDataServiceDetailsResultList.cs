using Newtonsoft.Json;

namespace Petrolcalculator.Core.Applications.Models.Json.DetailRequests.Implementation
{
    /// <summary>
    /// Result Data Model Root for Petrol Data Service Requests with Detail
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataServiceDetailsResultList
    {
        /// <summary>
        /// The Result
        /// </summary>
        [JsonProperty("station")]
        public PetrolDataServiceDetailResultModel Result { get; set; }
    }
}
