using System;
using Petrolcalculator.Core.Applications.Models.Web;

namespace Petrolcalculator.Core.Applications.Services.Web
{
    /// <summary>
    /// Service for Handling Paging
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IPagingService
    {
        /// <summary>
        /// Generates a List of Paging Elements based on the parameter
        /// </summary>
        /// <returns>List of Paging elements, without previous and next</returns>
        PagingModel GeneratePaging(Uri url, int pageNumber, int totalNumberOfElements);

        /// <summary>
        /// Genreates a string output of the current shown results related to all results
        /// </summary>
        /// <param name="page">current Page</param>
        /// <param name="totalNumberOfResults">Total Number Of Results</param>
        /// <returns>formated string with the information about the number of results</returns>
        string BuildResultOutputForCurrentPage(int page, int totalNumberOfResults);
    }
}
