using System;
using System.Linq;
using System.Net;
using NLog;
using Petrolcalculator.Core.Applications.Helper;
using Petrolcalculator.Core.Applications.Models;
using Petrolcalculator.Core.Applications.Models.Json.Geolocation;
using Petrolcalculator.Core.Applications.Services.Json;
using Petrolcalculator.Core.Applications.Services.Json.Implementations;

namespace Petrolcalculator.Core.Applications.Services.Search.Implementations
{
    /// <summary>
    /// Service to Map in put like Postcode, street, city to a valid geolocation
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class GoogleGeolocationMappingService : IGeolocationMappingService
    {
        #region Properties

        /// <summary>
        /// NLog
        /// </summary>
        private static readonly Logger Logger = Settings.Logging ? LogManager.GetCurrentClassLogger() : LogManager.CreateNullLogger();

        /// <summary>
        /// Json Mapping Service
        /// </summary>
        private readonly IJsonMapperService JsonMapper = new JsonMapperService();

        #endregion

        /// <summary>
        /// Takes a string and maps it to a geolocation via Google Geolocation Service
        /// </summary>
        /// <returns>a geoobject</returns>
        public Geoobject MapInputToGeolocation(string input)
        {
            // Get the Base Url
            string url = Settings.BaseGoolgeGeolocationServiceUrl;

            // Error Handling
            if (input.IsNullOrEmpty() || url.IsNullOrEmpty())
            {
                Logger.Info("MapInputToGeolocation: Url or Input is null");
                return new Geoobject();
            }

            // Build the Request Url
            string requestUri = $"{url}?address={Uri.EscapeDataString(input)}&region=de&sensor=false";

            // Call the google geolocation service
            string resultString;
            try
            {
                resultString = new WebClient().DownloadString(requestUri);
            }
            catch (Exception e)
            {
                Logger.Info("MapInputToGeolocation: " + e.Message);
                return new Geoobject();
            }

            // Map the Request Result to the generated Model
            GoogleGeocodingModel model = JsonMapper.MapJsonToClass<GoogleGeocodingModel>(resultString);

            // Check if the request was successfull
            if (!model.status.Equals(Labels.OkStatus))
            {
                Logger.Info("MapInputToGeolocation: Status:" + model.status);
                return new Geoobject();
            }

            // Get the information about lat and lng
            var firstGeocodeResult = model.results.FirstOrDefault();
            if (firstGeocodeResult == null)
            {
                Logger.Info("MapInputToGeolocation: No results in Model");
                return new Geoobject();
            }

            try
            {
                float lat = firstGeocodeResult.geometry.location.lat;
                float lng = firstGeocodeResult.geometry.location.lng;
                return new Geoobject(lat, lng);
            }
            catch (Exception e)
            {
                Logger.Info("MapInputToGeolocation: Could not retrieve lat/lng from result " + e.Message);
                return new Geoobject();
            }
        }

        /// <summary>
        /// Labels class for const strings
        /// </summary>
        public static class Labels
        {
            /// <summary>
            /// Geocoding OK status
            /// </summary>
            public const string OkStatus = "OK";
        }
    }
}
