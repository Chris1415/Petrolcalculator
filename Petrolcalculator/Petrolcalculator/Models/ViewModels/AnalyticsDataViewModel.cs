using System.Collections.Generic;
using System.Web.Mvc;
using Petrolcalculator.Core.Applications.Models.Statics;
using Petrolcalculator.Core.Applications.Models.Web;

namespace Petrolcalculator.Models.ViewModels
{
    /// <summary>
    /// View Model for the Analytics Result View
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class AnalyticsDataViewModel
    {
        /// <summary>
        /// Result List
        /// </summary>
        public IEnumerable<PetrolStationAnalyticsModel> AnalyticsData { get; set; }

        /// <summary>
        /// Droplist for Petrol Stations
        /// </summary>
        public IEnumerable<SelectListItem> PetrolStations { get; set; }

        /// <summary>
        /// Droplist for Days
        /// </summary>
        public IEnumerable<SelectListItem> Days { get; set; }

        /// <summary>
        /// Dicttionary with the analytics results
        /// Key is the kind of analytics
        /// Value is the string to be printed in frontend
        /// </summary>
        public Dictionary<string, string> AnalyticsEvaluationResults { get; set; } 
            
        /// <summary>
        /// List of Paging Elements to be displayed in frontend
        /// </summary>E
        public PagingModel Paging { get; set; } 

        /// <summary>
        /// The Output of the Result count
        /// </summary>
        public string ResultCountOutput { get; set; }
    }
}