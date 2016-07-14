using Newtonsoft.Json;

namespace Petrolcalculator.Core.Applications.Models.Json.PriceRequests.Implementation.Generic
{
    public class PetrolDataServicePriceResultList
    {
        /// <summary>
        /// List of Results
        /// </summary>
        [JsonProperty("prices")]
        public PetrolDataServicePriceResultStation Price { get; set; }
    }
}
