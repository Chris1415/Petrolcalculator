using Petrolcalculator.Core.Applications.Options.Base;

namespace Petrolcalculator.Core.Applications.Services.Search
{
    /// <summary>
    /// The Petrol Data Url Service
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IPetrolDataUrlService
    {
        /// <summary>
        /// Creates the Petrol Data Service URL based on the given options
        /// </summary>
        /// <typeparam name="T">options type</typeparam>
        /// <param name="baseUrl">base URL</param>
        /// <param name="options">given options</param>
        /// <returns>Response in JSON format</returns>
        string CreatePetrolDataServiceUrl<T>(string baseUrl, T options) where T : IPetrolDataOptions;
    }
}
