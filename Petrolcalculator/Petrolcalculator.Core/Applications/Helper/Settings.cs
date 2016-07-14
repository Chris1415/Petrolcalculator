using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace Petrolcalculator.Core.Applications.Helper
{
    /// <summary>
    /// Class to handle app.config Settings
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public static class Settings
    {
        /// <summary>
        /// Get the Flag for logging as bool
        /// </summary>
        public static bool Logging
        {
            get
            {
                const bool defaultValue = false;
                try
                {
                    string loggingValue = ConfigurationManager.AppSettings["Logging"] ?? string.Empty;
                    return Convert.ToBoolean(loggingValue);
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// Access the Petrol King Api Key
        /// </summary>
        public static string PetrolKingApiKey
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["TankerkoenigApiKey"] ?? string.Empty;
                }
                catch (ConfigurationErrorsException)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Access the Google Maps Api Key
        /// </summary>
        public static string GoogleMapsApiKey
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["GoogleMapsApiKey"] ?? string.Empty;
                }
                catch (ConfigurationErrorsException)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Access the Base List Url
        /// </summary>
        public static string BaseListUrl
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["BaseListUrl"] ?? string.Empty;
                }
                catch (ConfigurationErrorsException)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Access the Base Detail Url
        /// </summary>
        public static string BaseDetailUrl
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["BaseDetailUrl"] ?? string.Empty;
                }
                catch (ConfigurationErrorsException)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Access the Base Price Url
        /// </summary>
        public static string BasePriceUrl
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["BasePriceUrl"] ?? string.Empty;
                }
                catch (ConfigurationErrorsException)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Access the Base Price Url
        /// </summary>
        public static string BaseGoolgeGeolocationServiceUrl
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["BaseGoogleGeolocationService"] ?? string.Empty;
                }
                catch (ConfigurationErrorsException)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Access the Base Price Url
        /// </summary>
        public static int DistanceOfPetrolStationRequest
        {
            get
            {
                const int defaultValue = 5;
                try
                {
                    string distance = ConfigurationManager.AppSettings["DistanceOfPetrolStationRequest"] ?? string.Empty;
                    int distanceInInt = int.Parse(distance);
                    return distanceInInt >= 0 ? defaultValue : distanceInInt;
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// Access the Base Price Url
        /// </summary>
        public static int DelayBetweenTwoIdenticalRequests
        {
            get
            {
                const int defaultValue = 4;
                try
                {
                    string minutes = ConfigurationManager.AppSettings["DelayBetweenTwoIdenticalRequests"] ?? string.Empty;
                    int minutesInInt = int.Parse(minutes);
                    return minutesInInt <= 60 ? minutesInInt : defaultValue;
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// Access the Base Price Url
        /// </summary>
        public static int DelayWhileDataCollection
        {
            get
            {
                const int defaultValue = 1800000;
                try
                {
                    string delay = ConfigurationManager.AppSettings["DelayWhileDataCollection"] ?? string.Empty;
                    int delayInInt = int.Parse(delay);
                    return delayInInt <= 0 ? defaultValue : delayInInt;
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// Access the Petrol King Api Key
        /// </summary>
        public static IList<string> FavoritePetrolStations
        {
            get
            {
                try
                {
                    string petrolStationString = ConfigurationManager.AppSettings["FavoritePetrolStations"] ?? string.Empty;
                    return petrolStationString.Split('|').ToList();
                }
                catch (ConfigurationErrorsException)
                {
                    return new List<string>();
                }
            }
        }

        /// <summary>
        /// Determines the Path for the Serialization
        /// </summary>
        public static string AnaylticsSerializationPath
        {
            get
            {
                const string defaultValue = "Serialization/AnalyticsDataModel";
                try
                {
                    string pathFromWebRoot = ConfigurationManager.AppSettings["AnaylticsSerializationPathFromWebRoot"] ?? string.Empty;
                    return Path.Combine(HostingEnvironment.ApplicationPhysicalPath, pathFromWebRoot);
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// The Step With for Paging
        /// </summary>
        public static int ElementsPerPage
        {
            get
            {
                const int defaultValue = 20;
                try
                {
                    string elements = ConfigurationManager.AppSettings["ElementsPerPage"] ?? string.Empty;
                    int elementsInInt = int.Parse(elements);
                    return elementsInInt <= 0 ? defaultValue : elementsInInt;
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// The Number of Paging Elements to be printed
        /// </summary>
        public static int NumberOfPaingElements
        {
            get
            {
                const int defaultValue = 5;
                try
                {
                    string distance = ConfigurationManager.AppSettings["NumberOfPaingElements"] ?? string.Empty;
                    int distanceInInt = int.Parse(distance);
                    return distanceInInt <= 0 ? defaultValue : distanceInInt;
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }
    }
}
