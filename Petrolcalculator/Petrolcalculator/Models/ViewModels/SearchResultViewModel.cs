using Petrolcalculator.Core.Applications.Models.Json.ListRequests.Implementation;
using Petrolcalculator.Core.Applications.Options;

namespace Petrolcalculator.Models.ViewModels
{
    /// <summary>
    /// The View Model for the results in the search page
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class SearchResultViewModel
    {
        /// <summary>
        /// The Reuslt List
        /// </summary>
        public PetrolDataServiceResultList ResultList { get; set; }

        /// <summary>
        /// The given options
        /// </summary>
        public IPetrolDataListOptions Options { get; set; }
    }
}