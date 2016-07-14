using System.Collections.Generic;
using Newtonsoft.Json;

namespace Petrolcalculator.Core.Applications.Models.Json.ListRequests.Implementation
{
    /// <summary>
    /// Result Data Model Root for Petrol Data Service Requests with List
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataServiceResultList
    {
        /// <summary>
        /// List of Results
        /// </summary>
        [JsonProperty("stations")]
        public List<PetrolDataServiceResultModel> Results { get; set; }
    }
}
