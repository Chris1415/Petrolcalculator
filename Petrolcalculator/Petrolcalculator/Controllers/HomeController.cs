using System.Collections.Generic;
using System.Web.Mvc;
using NLog;
using Petrolcalculator.Core.Applications.Helper;
using Petrolcalculator.Core.Applications.Models;
using Petrolcalculator.Core.Applications.Models.Enums;
using Petrolcalculator.Core.Applications.Models.Json.ListRequests.Implementation;
using Petrolcalculator.Core.Applications.Options;
using Petrolcalculator.Core.Applications.Options.Implementations;
using Petrolcalculator.Core.Applications.Services.Facade;
using Petrolcalculator.Core.Applications.Services.Facade.Implementation;
using Petrolcalculator.Models.ViewModels;

namespace Petrolcalculator.Controllers
{
    /// <summary>
    /// Basic Home Controller
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class HomeController : Controller
    {
        #region Properties

        /// <summary>
        /// Service Facade
        /// </summary>
        private readonly IPetrolDataServiceFacade PetrolServiceFacade = new PetrolDataServiceFacade();

        /// <summary>
        /// NLog
        /// </summary>
        private static readonly Logger Logger = Settings.Logging ? LogManager.GetCurrentClassLogger() : LogManager.CreateNullLogger();

        #endregion

        #region RequestHandler

        /// <summary>
        /// Index View 
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Sort By Petrol Station Result Update Method
        /// </summary>
        /// <param name="sortBy">sort by Value</param>
        /// <param name="lat">Latitude</param>
        /// <param name="lng">Longtitude</param>
        /// <param name="petrolType">The Petrol Type</param>
        /// <returns>New Sorted Results</returns>
        public ActionResult UpdatePetrolStations(string sortBy, string lat, string lng, string petrolType)
        {
            double latDouble;
            double lngDouble;
            int sortByInt;
            int petrolTypeInt;

            // Check if parameter are valid
            if (sortBy.IsNullOrEmpty()
                || !int.TryParse(sortBy, out sortByInt)
                || petrolType.IsNullOrEmpty()
                || !int.TryParse(petrolType, out petrolTypeInt)
                || lat.IsNullOrEmpty()
                || !double.TryParse(lat.Replace(".", ","), out latDouble)
                || lng.IsNullOrEmpty()
                || !double.TryParse(lng.Replace(".", ","), out lngDouble))
            {
                Logger.Error("GeolocationMapper: input is invalid");
                return PartialView("PartialViews/PetrolServiceResultsList", EmptyViewModel);
            }

            // Build options for request based on the parameters
            IPetrolDataListOptions options = new PetrolDataListOptions()
            {
                GeoPosition = new Geoobject()
                {
                    Lat = latDouble,
                    Lng = lngDouble
                },
                PetrolType = (PetrolTypes)petrolTypeInt,
                Radius = Settings.DistanceOfPetrolStationRequest,
                SortOrder = (SortOrder)sortByInt
            };

            // Execute the request with the options via service facade
            PetrolDataServiceResultList result = PetrolServiceFacade.RequestPetrolStationList<PetrolDataServiceResultList>(options);
            // Return the results via view model
            return PartialView("PartialViews/PetrolServiceResultsList", CreateViewModel(options, result));
        }

        /// <summary>
        /// Geolocation Mapping function
        /// </summary>
        /// <param name="input">Input to be mapped</param>
        /// <returns>Json with the result geocoordinate</returns>
        [HttpPost]
        public JsonResult GeolocationMapper(string input)
        {
            // Check if parameter is valid
            if (input.IsNullOrEmpty())
            {
                Logger.Error("GeolocationMapper: input is null or empty");
                return Json(string.Empty);
            }

            // Execute the Request with the parameter via service facade
            Geoobject mappedGeoobject = PetrolServiceFacade.GeolocationMappingService.MapInputToGeolocation(input);
            // Return the result -> Geolocation as JSON if sucessfull, otherwise empty JSON string
            return mappedGeoobject.IsValid ? Json(mappedGeoobject) : Json(string.Empty);
        }

        #endregion

        #region Helper

        /// <summary>
        /// Create a empty view model, where no null ref exception can happen
        /// </summary>
        private static SearchResultViewModel EmptyViewModel => new SearchResultViewModel()
        {
            Options = new PetrolDataListOptions(),
            ResultList = new PetrolDataServiceResultList
            {
                Results = new List<PetrolDataServiceResultModel>()
            }
        };

        /// <summary>
        /// Create a View Model with options and results
        /// </summary>
        private static SearchResultViewModel CreateViewModel(IPetrolDataListOptions options, PetrolDataServiceResultList result)
        {
            return result == null || options == null
                ? EmptyViewModel
                : new SearchResultViewModel()
                {
                    Options = options,
                    ResultList = result
                };
        }

        #endregion
    }
}