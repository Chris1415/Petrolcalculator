using Petrolcalculator.Core.Applications.Attributes;

namespace Petrolcalculator.Core.Applications.Options.Base
{
    /// <summary>
    /// Options for Petrol Data Service Requests
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IPetrolDataOptions
    {
        #region Properties
        /// <summary>
        /// The APi Key
        /// </summary>
        [ParameterName("apikey")]
        string ApiKey { get; }

        #endregion Properties

        #region Helper

        /// <summary>
        /// Helper to check if the Model is valid for a Request
        /// </summary>
        /// <returns>true if the model is valid</returns>
        bool IsValid { get; }

        #endregion
    }
}
