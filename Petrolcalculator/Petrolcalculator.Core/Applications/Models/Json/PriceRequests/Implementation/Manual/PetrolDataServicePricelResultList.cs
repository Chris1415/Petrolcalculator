using System.Collections.Generic;

namespace Petrolcalculator.Core.Applications.Models.Json.PriceRequests.Implementation.Manual
{
    /// <summary>
    /// Result Data Model Root for Petrol Data Service Requests with Price
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataServicePriceResultList<T>
    {
        #region c'tor
        /// <summary>
        /// c'tor
        /// </summary>
        public PetrolDataServicePriceResultList()
        {
            Results = new Dictionary<string, T>();
        }

        #endregion

        /// <summary>
        /// List of Results
        /// Dictionary Key is the ID of the petrol station
        /// </summary>
        public Dictionary<string, T> Results { get; set; }
    }
}
