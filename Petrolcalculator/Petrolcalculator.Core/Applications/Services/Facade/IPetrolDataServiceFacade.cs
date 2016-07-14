using System;
using Petrolcalculator.Core.Applications.Models.Json.PriceRequests.Implementation.Manual;
using Petrolcalculator.Core.Applications.Options;
using Petrolcalculator.Core.Applications.Services.Analytics;
using Petrolcalculator.Core.Applications.Services.Base;
using Petrolcalculator.Core.Applications.Services.Json;
using Petrolcalculator.Core.Applications.Services.Search;

namespace Petrolcalculator.Core.Applications.Services.Facade
{
    /// <summary>
    /// Service Facade for Accessing the Petrol Data Services in a easy and common way
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IPetrolDataServiceFacade
    {
        #region Properties
        /// <summary>
        /// Petrol Data Service
        /// </summary>
        IPetrolDataService PetrolService { get; }

        /// <summary>
        /// Json Mapping Service
        /// </summary>
        IJsonMapperService JsonMappingService { get; }

        /// <summary>
        /// Petrol Data URL Service
        /// </summary>
        IPetrolDataUrlService PetrolDataUrlService { get; }

        /// <summary>
        /// Geolocation Mapping Service
        /// </summary>
        IGeolocationMappingService GeolocationMappingService { get; }

        /// <summary>
        /// Service for checking requests for restrictions
        /// </summary>
        IRestrictedPetrolDataRequestService RestrictedPetrolDataRequestService { get; }

        /// <summary>
        /// Json Serialization Service to serialize a model to HDD
        /// </summary>
        ISerializationService SerializationService { get; }

        /// <summary>
        /// Service, which tracks all running data collection threads
        /// </summary>
        IRunningThreadService RunningThreadService { get; }

        #endregion Properties

        #region Interface

        /// <summary>
        /// Get a List of all Petrol Stations which match the given options
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="options">given options</param>
        /// <returns>A List of Petrol stations</returns>
        T RequestPetrolStationList<T>(IPetrolDataListOptions options) where T : new();

        /// <summary>
        /// Get all prices of all Petrol Stations with given IDs
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="options">given options</param>
        /// <returns>A List of Petrol stations</returns>
        T RequestPetrolStationPrices<T>(IPetrolDataPriceOptions options) where T : new();

        /// <summary>
        /// Get all prices of all Petrol Stations with given IDs
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="options">given options</param>
        /// <returns>A List of Petrol stations</returns>
        PetrolDataServicePriceResultList<U> RequestPetrolStationPricesWithManualJsonSplitting<U>(IPetrolDataPriceOptions options) where U : new();

        /// <summary>
        /// Get all Details of all Petrol Stations with given IDs
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="options">given options</param>
        /// <returns>A List of Petrol stations</returns>
        T RequestPetrolStationDetails<T>(IPetrolDataDetailOptions options) where T : new();

        /// <summary>
        /// Interface function to start the continous price collection
        /// </summary>
        /// <param name="options">specific options</param>
        Guid CollectPetrolStationPrices(IPetrolDataPriceOptions options);

        /// <summary>
        /// Interface function to stop the current data collection 
        /// </summary>
        /// <param name="guid">guid of the data collection thhread</param>
        void StopCurrentDataCollection(Guid guid);

        #endregion
    }
}
