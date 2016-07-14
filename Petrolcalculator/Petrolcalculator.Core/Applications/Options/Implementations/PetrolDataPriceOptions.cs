using System.Collections.Generic;
using System.Linq;
using Petrolcalculator.Core.Applications.Helper;

namespace Petrolcalculator.Core.Applications.Options.Implementations
{
    /// <summary>
    /// Petrol Data Service Options for Price Requests
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataPriceOptions : IPetrolDataPriceOptions
    {
        public PetrolDataPriceOptions()
        {
            ApiKey = Settings.PetrolKingApiKey;
        }

        /// <summary>
        /// The APi Key
        /// </summary>
        public string ApiKey { get; }

        /// <summary>
        /// Specific Petrol Station IDs to call
        /// </summary>
        public IEnumerable<string> Ids { get; set; }

        #region Helper

        /// <summary>
        /// Helper to check if the Model is valid for a Request
        /// </summary>
        /// <returns>true if the model is valid</returns>
        public bool IsValid => !ApiKey.IsNullOrEmpty() 
            && Ids != null
            && Ids.Any();

        #endregion 
    }
}
