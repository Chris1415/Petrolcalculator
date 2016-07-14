using System;
using System.Collections.Generic;
using System.Linq;
using Petrolcalculator.Core.Applications.Models.Json.PriceRequests.Implementation;
using Petrolcalculator.Core.Applications.Models.Json.PriceRequests.Implementation.Manual;
using Petrolcalculator.Core.Applications.Models.Statics;

namespace Petrolcalculator.Core.Applications.Helper
{
    /// <summary>
    /// Extension for Model Mappings
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public static class ModelMappingExtension
    {
        /// <summary>
        /// Maps the Request List Results to the Analytics List Model
        /// </summary>
        /// <param name="input">request list result </param>
        /// <returns>The mapped analytics List Model</returns>
        public static IList<PetrolStationAnalyticsModel> ToAnalyticsList(
            this PetrolDataServicePriceResultList<PetrolDataServicePriceResultModel> input)
        {
            return input.Results.Select(result => result.Value.ToAnalyticsModel(result.Key)).ToList();
        }

        /// <summary>
        /// Maps a Single Request Result to a single analytics mode element
        /// </summary>
        /// <param name="input">single request result</param>
        /// <param name="guid">guid of the result</param>
        /// <returns>single mapped analytics model</returns>
        public static PetrolStationAnalyticsModel ToAnalyticsModel(this PetrolDataServicePriceResultModel input,
            string guid)
        {
            return new PetrolStationAnalyticsModel()
            {
                Diesel = input.Diesel,
                E5 = input.E5,
                E10 = input.E10,
                Status = input.Status,
                PetrolStationId = guid,
                RequestDatetime = DateTime.Now
            };
        }
    }
}
