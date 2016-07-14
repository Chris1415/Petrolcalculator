using System;
using Petrolcalculator.Core.Applications.Models.Json.PriceRequests.Implementation;

namespace Petrolcalculator.Core.Applications.Models.Statics
{
    /// <summary>
    /// The Petrol Station Analytics Model for long term price observation
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    [Serializable]
    public class PetrolStationAnalyticsModel : PetrolDataServicePriceResultModel
    {
        /// <summary>
        /// The Datetime of the Request
        /// </summary>
        public DateTime RequestDatetime { get; set; }

        /// <summary>
        /// The PetrolStation Id
        /// </summary>
        public string PetrolStationId { get; set; }
    }
}
