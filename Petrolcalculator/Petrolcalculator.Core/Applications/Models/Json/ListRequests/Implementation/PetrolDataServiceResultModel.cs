using Newtonsoft.Json;

namespace Petrolcalculator.Core.Applications.Models.Json.ListRequests.Implementation
{
    /// <summary>
    /// Result Data Model for Petrol Data Service Requests
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataServiceResultModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        [JsonProperty("lat")]
        public string Lat { get; set; }

        /// <summary>
        /// Longtitude
        /// </summary>
        [JsonProperty("lng")]
        public string Lng { get; set; }

        /// <summary>
        /// Brand
        /// </summary>
        [JsonProperty("brand")]
        public string Brand { get; set; }

        /// <summary>
        /// Distance
        /// </summary>
        [JsonProperty("dist")]
        public string Dist { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [JsonProperty("price")]
        public string Price { get; set; }

        /// <summary>
        /// Station Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Street
        /// </summary>
        [JsonProperty("street")]
        public string Street { get; set; }

        /// <summary>
        /// Housnumber
        /// </summary>
        [JsonProperty("houseNumber")]
        public string HousNumber { get; set; }

        /// <summary>
        /// PostCode
        /// </summary>
        [JsonProperty("postCode")]
        public string PostCode { get; set; }

        /// <summary>
        /// Place
        /// </summary>
        [JsonProperty("place")]
        public string Place { get; set; }

        /// <summary>
        /// Is Open Flag
        /// </summary>
        [JsonProperty("isOpen")]
        public string IsOpen { get; set; }

        /// <summary>
        /// e5 Price when all was requested
        /// </summary>
        [JsonProperty("e5")]
        public string E5 { get; set; }

        /// <summary>
        /// e10 Price when all was requested
        /// </summary>
        [JsonProperty("e10")]
        public string E10 { get; set; }

        /// <summary>
        /// diesel Price when all was requested
        /// </summary>
        [JsonProperty("diesel")]
        public string Diesel { get; set; }
    }
}
