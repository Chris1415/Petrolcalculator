using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NLog;
using Petrolcalculator.Core.Applications.Helper;
using Petrolcalculator.Core.Applications.Models.Statics;
using Petrolcalculator.Core.Applications.Models.Web;
using Petrolcalculator.Core.Applications.Options;
using Petrolcalculator.Core.Applications.Options.Implementations;
using Petrolcalculator.Core.Applications.Repositories.Implementation;
using Petrolcalculator.Core.Applications.Services.Facade;
using Petrolcalculator.Core.Applications.Services.Facade.Implementation;
using Petrolcalculator.Models.ViewModels;

namespace Petrolcalculator.Controllers
{
    /// <summary>
    /// Statistik Controller, for the statistik functionality
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class AnalyticsController : Controller
    {
        #region Properties

        /// <summary>
        /// Petrol Service Facade
        /// </summary>
        private readonly IPetrolDataServiceFacade PetrolServiceFacade = new PetrolDataServiceFacade();

        /// <summary>
        /// Analytcis Service Facade
        /// </summary>
        private readonly IAnalyticsServiceFacade AnalyticsServiceFacade = new AnalyticsServiceFacade();

        /// <summary>
        /// Web Service Facade
        /// </summary>
        private readonly IWebServiceFacade WebServiceFacade = new WebServiceFacade();

        /// <summary>
        /// NLog
        /// </summary>
        private static readonly Logger Logger = Settings.Logging ? LogManager.GetCurrentClassLogger() : LogManager.CreateNullLogger();

        #endregion

        #region RequestHandler

        /// <summary>
        /// Index Method
        /// </summary>
        /// <returns>Index View</returns>
        public ActionResult Index()
        {
            // Get the information, if a datacollection thread is already running -> Session entry at the specified key
            object dataCollectionThreadGuid = Session[Labels.DataCollectionSessionKey];
            // Return the Result in a view model
            return View(CreateAnalyticsPageViewModel(dataCollectionThreadGuid != null));
        }

        /// <summary>
        /// Action to start the analytics data collection in a background thread
        /// </summary>
        /// <returns>Guid of the data collection thread</returns>
        public bool StartDataCollection()
        {
            // Build the options for the request
            // Options are taken from the web config entry "FavoritePetrolStations"
            IPetrolDataPriceOptions options = new PetrolDataPriceOptions()
            {
                Ids = Settings.FavoritePetrolStations
            };

            // Request the Data Collection via facade and get the guid of the thread
            string threadGuid = PetrolServiceFacade.CollectPetrolStationPrices(options).ToString();
            // Save the guid in the session
            Session[Labels.DataCollectionSessionKey] = threadGuid;
            return true;
        }

        /// <summary>
        /// Action to stop the analytics data collection
        /// </summary>
        /// <returns>true if the termination was successful</returns>
        public bool StopDataCollection()
        {
            try
            {
                // Get the saved guid from session for running data collection thread
                object dataCollectionThreadGuid = Session[Labels.DataCollectionSessionKey];
                if (dataCollectionThreadGuid == null)
                {
                    Logger.Error("StopDataCollection: Data Collection Thread GUID is null");
                    return false;
                }

                // Build the Guid from session entry
                Guid mappedGuid = new Guid(dataCollectionThreadGuid.ToString());
                // Request thread Stop via service facade
                PetrolServiceFacade.StopCurrentDataCollection(mappedGuid);
                // Delete the Session entry
                Session[Labels.DataCollectionSessionKey] = null;
                return true;
            }
            catch (Exception e)
            {
                Logger.Error("StopDataCollection: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Update Graph Data Method
        /// </summary>
        /// <param name="petrolStationId">chosen Petrol Station</param>
        /// <param name="requestDay">requested Day</param>
        /// <returns>Json with all results, which fit the filter</returns>
        [HttpPost]
        public JsonResult UpdateGraphData(string petrolStationId = "All", string requestDay = "All")
        {
            // Determine if the special filter All is set
            bool filterPetrolStationsByAll = petrolStationId.Equals(Labels.AllValue);
            bool filterDaysByAll = requestDay.Equals(Labels.AllValue);

            // First get all Data
            IEnumerable<PetrolStationAnalyticsModel> analyticsData = PetrolStationAnalyticsRepository.AnalyticsEntries;
            if (analyticsData == null)
            {
                Logger.Error("UpdateGraphData: analyticsdata is null");
                return Json(string.Empty);
            }

            if (!filterPetrolStationsByAll)
            {
                // If an ID is chosen, filter all results by id
                analyticsData = analyticsData
                    .Where(element => element.PetrolStationId.Equals(petrolStationId));
            }

            if (!filterDaysByAll)
            {
                // If a day is chosen, filter all results by day
                analyticsData = analyticsData
                    .Where(element => element.RequestDatetime.ToString(Labels.DataFormat).Equals(requestDay));
            }

            return Json(analyticsData);
        }

        /// <summary>
        /// Action to update the analytics raw result printing
        /// </summary>
        /// <returns>Anayltics Data View</returns>
        public ActionResult UpdateAnalytics(string petrolStationId = "All", string requestDay = "All", int page = 1)
        {
            // Determine if the special filter All is set
            bool filterPetrolStationsByAll = petrolStationId.Equals(Labels.AllValue);
            bool filterDaysByAll = requestDay.Equals(Labels.AllValue);

            // First get all Data
            IEnumerable<PetrolStationAnalyticsModel> analyticsData = PetrolStationAnalyticsRepository.AnalyticsEntries;
            if (analyticsData == null)
            {
                Logger.Error("UpdateAnalytics: analyticsdata is null");
                return PartialView("PartialViews/AnalyticsData", EmptyAnalyticsDataViewModel);
            }

            // Build the Dropdown list for Petrol station choice
            IEnumerable<SelectListItem> petrolStationDropDown = BuildDropDown(
                analyticsData,
                element => element.PetrolStationId,
                petrolStationId, filterPetrolStationsByAll,
                Labels.TextForAllPetrolStations);
            // Build the Dropdown list for days choice
            IEnumerable<SelectListItem> daysDropDown = BuildDropDown(
                analyticsData,
                element => element.RequestDatetime.ToString(Labels.DataFormat),
                requestDay, filterDaysByAll,
                Labels.TextForAllDays);

            if (!filterPetrolStationsByAll)
            {
                // If an ID is chosen, filter all results by id
                analyticsData = analyticsData
                    .Where(element => element.PetrolStationId.Equals(petrolStationId));
            }

            if (!filterDaysByAll)
            {
                // If a day is chosen, filter all results by day
                analyticsData = analyticsData
                    .Where(element => element.RequestDatetime.ToString(Labels.DataFormat).Equals(requestDay));
            }

            // Apply post analytics Evaluations
            Dictionary<string, string> analyticsResults = this.AnalyticsServiceFacade.EvaluateAllAnalytics(analyticsData);

            // Get the current Total Number of results
            int currentTotalNumberOfResults = analyticsData.Count();

            // Build the Paging Elements for Fronten
            // Check if the page number is invalid, if so use default page 1
            page = page <= 0 ? 1 : page;
            // Execute the Paging Service
            PagingModel pagingModel = WebServiceFacade.PagingService.GeneratePaging(Request.Url, page, currentTotalNumberOfResults);

            //Get the Number Of Results output for current Page
            string resultCountOutput = WebServiceFacade.PagingService.BuildResultOutputForCurrentPage(page, currentTotalNumberOfResults);

            //PostFiltering the results
            analyticsData = analyticsData.Skip((page - 1) * Settings.ElementsPerPage).Take(Settings.ElementsPerPage);

            // Return the view with the viewmodel
            return PartialView("PartialViews/AnalyticsData", CreateAnalyticsDataViewModel(
                analyticsData,
                petrolStationDropDown,
                daysDropDown,
                analyticsResults,
                pagingModel,
                resultCountOutput));
        }

        #endregion

        #region Helper

        /// <summary>
        /// Helper to create a Dropdown List based on the parameter
        /// </summary>
        /// <param name="analyticsData">the analytics data</param>
        /// <param name="predicate">The predicate to define which field should be used for the dropdown</param>
        /// <param name="identifier">identifier to get the selected entry</param>
        /// <param name="filterByAll">check if filter by all is selected</param>
        /// <param name="textForAllElement">text for filter by all</param>
        /// <returns>a filled dropdown list</returns>
        private static IEnumerable<SelectListItem> BuildDropDown(
            IEnumerable<PetrolStationAnalyticsModel> analyticsData,
            Func<PetrolStationAnalyticsModel, string> predicate,
            string identifier,
            bool filterByAll,
            string textForAllElement = "all")
        {
            // Build the droplist entries with all avaiilable elements and the chosen one as selcected
            IList<SelectListItem> dropDownList = analyticsData
                .Select(predicate)
                .Distinct()
                .Select(element => new SelectListItem()
                {
                    Selected = identifier.Equals(element),
                    Text = element,
                    Value = element
                })
                .ToList();

            // Build the "All" Element
            SelectListItem allItem = new SelectListItem()
            {
                Selected = filterByAll,
                Text = textForAllElement,
                Value = Labels.AllValue
            };

            // Add the "All" Element to the dropdown list
            dropDownList.Add(allItem);
            return dropDownList;
        }

        /// <summary>
        /// Create a View Model with page options and results
        /// </summary>
        /// <param name="isDataCollectionThreadRunning">flag to determine if the datacollection thread is runnin</param>
        /// <returns>analyticspageviewmodel</returns>
        private static AnalyticsPageViewModel CreateAnalyticsPageViewModel(bool isDataCollectionThreadRunning)
        {
            return new AnalyticsPageViewModel()
            {
                IsDataCollectionThread = isDataCollectionThreadRunning
            };
        }

        /// <summary>
        /// Create a View Model with options and results
        /// </summary>
        /// <param name="analyticsData">the analytics data</param>
        /// <param name="petrolStations">the list of petrol station dropdownentries</param>
        /// <param name="days">the list of days dropdownentries</param>
        /// <param name="analyticsDictionary">Dictionary with the analytics results</param>
        /// <param name="pagingElements">The Paging Elements</param>
        /// <param name="resultCountOutput">The output of the result count</param>
        /// <returns>analyticsdataviewmodel</returns>
        private static AnalyticsDataViewModel CreateAnalyticsDataViewModel(
            IEnumerable<PetrolStationAnalyticsModel> analyticsData,
            IEnumerable<SelectListItem> petrolStations,
            IEnumerable<SelectListItem> days,
            Dictionary<string, string> analyticsDictionary,
            PagingModel pagingElements,
            string resultCountOutput)
        {
            if (analyticsData == null || petrolStations == null || days == null)
            {
                Logger.Error("CreateAnalyticsDataViewModel: necessary data are null");
                return EmptyAnalyticsDataViewModel;
            }

            return new AnalyticsDataViewModel()
            {
                AnalyticsData = analyticsData,
                PetrolStations = petrolStations,
                Days = days,
                AnalyticsEvaluationResults = analyticsDictionary,
                Paging = pagingElements,
                ResultCountOutput = resultCountOutput
            };
        }

        /// <summary>
        /// Create a empty view model, where no null ref exception can happen
        /// </summary>
        private static AnalyticsDataViewModel EmptyAnalyticsDataViewModel => new AnalyticsDataViewModel()
        {
            AnalyticsData = new List<PetrolStationAnalyticsModel>(),
            PetrolStations = new List<SelectListItem>(),
            Days = new List<SelectListItem>(),
            AnalyticsEvaluationResults = new Dictionary<string, string>(),
            Paging = new PagingModel()
        };

        #endregion

        /// <summary>
        /// Const Labels
        /// </summary>
        public static class Labels
        {
            /// <summary>
            /// The chosen Dateformat
            /// </summary>
            public const string DataFormat = "dd MMMM";

            /// <summary>
            /// Text for the All Entry of the Petrol Station Dropdown
            /// </summary>
            public const string TextForAllPetrolStations = "All Petrol Stations";

            /// <summary>
            /// Text for the All Entry of the Day Dropdown
            /// </summary>
            public const string TextForAllDays = "All Days";

            /// <summary>
            /// All String
            /// Special Filter with analytics data
            /// </summary>
            public const string AllValue = "All";

            /// <summary>
            /// Session Key of the DataCollection Thread GUID
            /// </summary>
            public const string DataCollectionSessionKey = "DataCollectionThreadGuid";
        }
    }
}