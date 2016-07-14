namespace Petrolcalculator.Core.Applications.Services.Search
{
    /// <summary>
    /// The Petrol Data Service to get all necessary Data
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IPetrolDataService
    {
        /// <summary>
        /// Requests the petrol Service
        /// </summary>
        /// <param name="url">concrete URL for the Request</param>
        /// <returns>JSON String with a List of all Petrol Stations, which fit to the request options</returns>
        string RequestPetrolService(string url);
    }
}
