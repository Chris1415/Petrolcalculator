using System;
using System.Net;
using NLog;
using Petrolcalculator.Core.Applications.Helper;

namespace Petrolcalculator.Core.Applications.Services.Search.Implementations
{
    /// <summary>
    /// The Petrol Data Service to get all necessary Data
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataService : IPetrolDataService
    {
        #region Properties

        /// <summary>
        /// NLog
        /// </summary>
        private static readonly Logger Logger = Settings.Logging ? LogManager.GetCurrentClassLogger() : LogManager.CreateNullLogger();

        #endregion

        #region Interface

        /// <summary>
        /// Requests the petrol Service
        /// </summary>
        /// <param name="url">concrete URL for the Request</param>
        /// <returns>JSON String with a List of all Petrol Stations, which fit to the request options</returns>
        public string RequestPetrolService(string url)
        {
            try
            {
                using (var client = new WebClient())
                {
                    return client.DownloadString(url);
                }
            }
            catch (Exception e)
            {
                Logger.Error("RequestPetrolService: " + e.Message);
                return string.Empty;
            }
           
        }

        #endregion
    }
}
