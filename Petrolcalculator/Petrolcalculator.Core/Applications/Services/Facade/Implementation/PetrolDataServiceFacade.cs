using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Linq;
using NLog;
using Petrolcalculator.Core.Applications.Helper;
using Petrolcalculator.Core.Applications.Models.Json.PriceRequests.Implementation;
using Petrolcalculator.Core.Applications.Models.Json.PriceRequests.Implementation.Manual;
using Petrolcalculator.Core.Applications.Models.Statics;
using Petrolcalculator.Core.Applications.Options;
using Petrolcalculator.Core.Applications.Repositories.Implementation;
using Petrolcalculator.Core.Applications.Services.Analytics;
using Petrolcalculator.Core.Applications.Services.Analytics.Implementations;
using Petrolcalculator.Core.Applications.Services.Base;
using Petrolcalculator.Core.Applications.Services.Json;
using Petrolcalculator.Core.Applications.Services.Json.Implementations;
using Petrolcalculator.Core.Applications.Services.Search;
using Petrolcalculator.Core.Applications.Services.Search.Implementations;

namespace Petrolcalculator.Core.Applications.Services.Facade.Implementation
{
    /// <summary>
    /// Service Facade for Accessing the Petrol Data Services in a easy and common way
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataServiceFacade : IPetrolDataServiceFacade
    {
        #region Properties
        /// <summary>
        /// Petrol Data Service
        /// </summary>
        public IPetrolDataService PetrolService { get; }

        /// <summary>
        /// Json Mapping Service
        /// </summary>
        public IJsonMapperService JsonMappingService { get; }

        /// <summary>
        /// Petrol Data URL Service
        /// </summary>
        public IPetrolDataUrlService PetrolDataUrlService { get; }

        /// <summary>
        /// Geolocation Mapping Service
        /// </summary>
        public IGeolocationMappingService GeolocationMappingService { get; }

        /// <summary>
        /// Service for checking requests for restrictions
        /// </summary>
        public IRestrictedPetrolDataRequestService RestrictedPetrolDataRequestService { get; }

        /// <summary>
        /// Service, which tracks all running data collection threads
        /// </summary>
        public IRunningThreadService RunningThreadService { get; }

        /// <summary>
        /// Json Serialization Service to serialize a model to HDD
        /// </summary>
        public ISerializationService SerializationService { get; }

        /// <summary>
        /// NLog
        /// </summary>
        private static readonly Logger Logger = Settings.Logging ? LogManager.GetCurrentClassLogger() : LogManager.CreateNullLogger();

        #endregion

        #region Interface

        /// <summary>
        /// Get a List of all Petrol Stations which match the given options
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="options">given options</param>
        /// <returns>A List of Petrol stations</returns>
        public T RequestPetrolStationList<T>(IPetrolDataListOptions options) where T : new()
        {
            // Check if the options are valid for a request
            if (!options.IsValid)
            {
                Logger.Error("RequestPetrolStationList: options are invalid");
                return default(T);
            }

            // Check if this specific Request is restricted
            T result = RestrictedPetrolDataRequestService.CheckRequest<T>(options);
            // If a non null result comes back, the request was restricted and gives cached results back
            if (result != null)
            {
                return result;
            }

            // If the results was null, the request is not restricted, execute the request
            string url = PetrolDataUrlService.CreatePetrolDataServiceUrl(Settings.BaseListUrl, options);
            T results = RequestPetrolStationWithUrl<T>(url);
            if (results != null)
            {
                RestrictedPetrolDataRequestService.AddRequestWithResult(options, results);
            }

            return results;
        }

        /// <summary>
        /// Get all prices of all Petrol Stations with given IDs
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="options">given options</param>
        /// <returns>A List of Petrol stations</returns>
        public T RequestPetrolStationPrices<T>(IPetrolDataPriceOptions options) where T : new()
        {
            // Check if the options are valid for a request
            if (!options.IsValid)
            {
                Logger.Error("RequestPetrolStationPrices: options are invalid");
                return default(T);
            }

            // Create the specific Request Url
            string url = PetrolDataUrlService.CreatePetrolDataServiceUrl(Settings.BasePriceUrl, options);
            // Execute the Reuqest
            return RequestPetrolStationWithUrl<T>(url);
        }

        /// <summary>
        /// Get all prices of all Petrol Stations with given IDs
        /// </summary>
        /// <typeparam name="TU">Model Result Type</typeparam>
        /// <param name="options">given options</param>
        /// <returns>A List of Petrol stations</returns>
        public PetrolDataServicePriceResultList<TU> RequestPetrolStationPricesWithManualJsonSplitting<TU>(IPetrolDataPriceOptions options) where TU : new()
        {
            PetrolDataServicePriceResultList<TU> results = new PetrolDataServicePriceResultList<TU>();

            // Check if the options are valid for a request
            if (!options.IsValid)
            {
                Logger.Error("RequestPetrolStationPricesWithManualJsonSplitting: options are invalid");
                return results;
            }

            // Create the specific Request Url
            string url = PetrolDataUrlService.CreatePetrolDataServiceUrl(Settings.BasePriceUrl, options);

            // Because of inconsistent REST API the Mapping of the retrieved JSON has to be changed
            string json = PetrolService.RequestPetrolService(url);
            if (!SuccessfullRequest(json))
            {
                Logger.Info("RequestPetrolStationPricesWithManualJsonSplitting: Request was not successfull");
                return results;
            }

            // Split the incoming JSON into its stations and save them manually
            // After skipping the Ids, generic mapping can be executed again
            IEnumerable<string> stationJsons = SplitPriceJsonIntoStations(json, options);
            for (int index = 0; index < stationJsons.Count(); index++)
            {
                TU result = JsonMappingService.MapJsonToClass<TU>(stationJsons.ElementAt(index));
                if (result != null)
                {
                    results.Results.Add(options.Ids.ElementAt(index), result);
                }
            }

            return results;
        }

        /// <summary>
        /// Get all Details of all Petrol Stations with given IDs
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="options">given options</param>
        /// <returns>A List of Petrol stations</returns>
        public T RequestPetrolStationDetails<T>(IPetrolDataDetailOptions options) where T : new()
        {
            // Check if the options are valid for a request
            if (!options.IsValid)
            {
                Logger.Info("RequestPetrolStationDetails: Options are invalid");
                return default(T);
            }

            // Create the specific Request Url
            string url = PetrolDataUrlService.CreatePetrolDataServiceUrl(Settings.BaseDetailUrl, options);
            //Execute the Request
            return RequestPetrolStationWithUrl<T>(url);
        }

        /// <summary>
        /// Interface function to start the continous price collection
        /// </summary>
        /// <param name="options">specific options</param>
        public Guid CollectPetrolStationPrices(IPetrolDataPriceOptions options)
        {
            // Check if the options are valid for a request
            if (!options.IsValid)
            {
                Logger.Error("CollectPetrolStationPrices: options are invalid");
                return Guid.Empty;
            }

            // Create a new Guid
            Guid threadGuid = Guid.NewGuid();
            // Create a Datacollection Thread
            Thread dataCollectionThread = new Thread(() => DataCollectionLoop(options, threadGuid));
            // Insert the thread with the guid into the static repository for tracking
            RunningThreadService.AddNewDataCollectionThread(dataCollectionThread, threadGuid);
            // Start the Collection
            dataCollectionThread.Start();
            // Give the GUID back to the UI for further interaction and tracking
            return threadGuid;
        }

        /// <summary>
        /// Interface function to stop the current data collection 
        /// </summary>
        /// <param name="guid">guid of the data collection thhread</param>
        public void StopCurrentDataCollection(Guid guid)
        {
            // Save Way to shutdown a thread with the given guid
            RunningThreadService.ForceShutDown(guid);
        }

        #endregion

        #region c'tor

        /// <summary>
        /// Default c'tor with default initializations
        /// </summary>
        public PetrolDataServiceFacade()
        {
            PetrolService = new PetrolDataService();
            JsonMappingService = new JsonMapperService();
            PetrolDataUrlService = new PetrolDataUrlService();
            GeolocationMappingService = new GoogleGeolocationMappingService();
            RestrictedPetrolDataRequestService = new RestrictedPetrolDataRequestService();
            RunningThreadService = new RunningThreadService();
            SerializationService = new JsonSerializationService();
        }

        /// <summary>
        /// c'tor with Parameters
        /// Note: Here DI Tool Simple Injector could be used
        /// </summary>
        /// <param name="petrolService">Petrol Data Service</param>
        /// <param name="jsonMappingService">Json Mapping Service</param>
        /// <param name="petrolDataUrlService">Petrol Data Url Service</param>
        /// <param name="geolocationMappingService">Geolocation Mapping Service</param>
        /// <param name="restrictedPetrolDataRequestService">Checker for Restricted Requests</param>
        /// <param name="runningThreadService">Tracks all Running data collection threads</param>
        /// <param name="serializationService">Serialization Service</param>
        public PetrolDataServiceFacade(
            IPetrolDataService petrolService,
            IJsonMapperService jsonMappingService,
            IPetrolDataUrlService petrolDataUrlService,
            IGeolocationMappingService geolocationMappingService,
            IRestrictedPetrolDataRequestService restrictedPetrolDataRequestService,
            IRunningThreadService runningThreadService,
            ISerializationService serializationService)
        {
            PetrolService = petrolService;
            JsonMappingService = jsonMappingService;
            PetrolDataUrlService = petrolDataUrlService;
            GeolocationMappingService = geolocationMappingService;
            RestrictedPetrolDataRequestService = restrictedPetrolDataRequestService;
            RunningThreadService = runningThreadService;
            SerializationService = serializationService;
        }

        #endregion

        #region Helper

        /// <summary>
        /// Call the Reqeust with a given URL
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="url">url to call</param>
        /// <returns>Filled Result Type</returns>
        private T RequestPetrolStationWithUrl<T>(string url) where T : new()
        {
            string json = PetrolService.RequestPetrolService(url);
            return SuccessfullRequest(json)
                ? JsonMappingService.MapJsonToClass<T>(json) 
                : default(T);
        }

        /// <summary>
        /// Helper to split the Json manually into stations to skip the changing Ids, which make generic mapping hard
        /// </summary>
        /// <param name="stationJson">station json</param>
        /// <param name="options">options with IDs</param>
        /// <returns>a single json for each station</returns>
        private static IEnumerable<string> SplitPriceJsonIntoStations(string stationJson, IPetrolDataPriceOptions options)
        {
            // Parse the results in a dynamic Json Value
            dynamic data = JObject.Parse(stationJson);

            // Get all requested IDs from the Options and give the values of that ids in a list back
            return from id
                in options.Ids
                   where data[Labels.PricesJsonKey][id] != null
                   select data[Labels.PricesJsonKey][id].ToString()
                into stationData
                   select (string)stationData;
        }

        /// <summary>
        /// Check the incoming JSON for an entry ok and check if that entry is true or false
        /// </summary>
        /// <param name="json">json</param>
        /// <returns>true if the json ok flag is true, otherwise false</returns>
        private static bool SuccessfullRequest(string json)
        {
            try
            {
                // Parse the Json as Dynamic value
                dynamic data = JObject.Parse(json);
                // Get the Ok field and check if the value is the Error Value

                bool returnValue = data.ok != null
                       && !data.ok.ToString().Equals(Labels.RequestErrorValue)
                       && !data.ok.ToString().Equals(Labels.RequestErrorValue.ToLower());

                if (!returnValue)
                {
                    Logger.Error("SuccessfullRequest: " + data.message.ToString());
                }

                return returnValue;
            }
            catch (Exception e)
            {
                Logger.Error("SuccessfullRequest: " + e.Message);
                return false;
            }
           
        }

        /// <summary>
        /// The Loop which gets the Price every x minutes
        /// x can be adjusted via web.config entry "DelayWhileDataCollection"
        /// </summary>
        /// <param name="options">specific request options</param>
        /// <param name="threadGuid">the guid of the current thread</param>
        private void DataCollectionLoop(IPetrolDataPriceOptions options, Guid threadGuid)
        {
            // Check if the shut down should be forced
            while (!RunningThreadService.ShutDownForced(threadGuid))
            {
                // Get the station data from json dynamically based on the ids of the input
                PetrolDataServicePriceResultList<PetrolDataServicePriceResultModel> results = RequestPetrolStationPricesWithManualJsonSplitting<PetrolDataServicePriceResultModel>(options);

                // Get the station data from station generic, but static with predefined static Station id in JsonProperty
                // PetrolDataServicePriceResultList resultsGeneric = RequestPetrolStationPrices<PetrolDataServicePriceResultList>(options);

                // Map the Request Result to Analytics List Model
                // Then Filter all results of closed Petrol Stations
                foreach (PetrolStationAnalyticsModel model in results
                    .ToAnalyticsList()
                    .Where(element => element.Status.Equals(Labels.OpenStore)))
                {
                    PetrolStationAnalyticsRepository.AnalyticsEntries.Add(model);
                }

                // Serialize the Result
                SerializationService.Serialize(PetrolStationAnalyticsRepository.AnalyticsEntries, Settings.AnaylticsSerializationPath);

                // Sleep again
                Thread.Sleep(Settings.DelayWhileDataCollection);
            }
            // Remove this thread from the static list
            Logger.Info("DataCollectionLoop: shutdown triggered");
            RunningThreadService.RemoveDataCollectionThread(threadGuid);
        }

        #endregion

        /// <summary>
        /// Labels class for const strings
        /// </summary>
        public class Labels
        {
            /// <summary>
            /// Prices key of the json result
            /// </summary>
            public const string PricesJsonKey = "prices";

            /// <summary>
            /// Value of the Json Result if the request was not successful
            /// </summary>
            public const string RequestErrorValue = "False";

            /// <summary>
            /// String which represents an open store in the json result
            /// </summary>
            public const string OpenStore = "open";
        }
    }
}