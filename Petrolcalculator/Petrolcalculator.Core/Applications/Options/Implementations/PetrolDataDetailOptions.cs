using Petrolcalculator.Core.Applications.Helper;

namespace Petrolcalculator.Core.Applications.Options.Implementations
{
    /// <summary>
    /// Petrol Data Service Options for List Requests
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataDetailOptions : IPetrolDataDetailOptions
    {
        #region c'tor
        /// <summary>
        /// c'tor
        /// </summary>
        public PetrolDataDetailOptions()
        {
            ApiKey = Settings.PetrolKingApiKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Id of a petrol Station
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Api Key
        /// </summary>
        public string ApiKey { get; }

        #endregion

        #region Helper

        /// <summary>
        /// Helper to check if the Model is valid for a Request
        /// </summary>
        /// <returns>true if the model is valid</returns>
        public bool IsValid => !ApiKey.IsNullOrEmpty()
                               && Id.IsNullOrEmpty();

        #endregion
    }
}
