using System;
using System.Collections.Generic;
using Petrolcalculator.Core.Applications.Helper;
using Petrolcalculator.Core.Applications.Models.Web;

namespace Petrolcalculator.Core.Applications.Services.Web.Implementations
{
    /// <summary>
    /// Service for Handling Paging
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PagingService : IPagingService
    {
        #region Properties

        #endregion

        #region Interface

        /// <summary>
        /// Generates a List of Paging Elements based on the parameter
        /// </summary>
        /// <returns>List of Paging elements, without previous and next</returns>
        public PagingModel GeneratePaging(Uri url, int pageNumber, int totalNumberOfElements)
        {
            int lastElement = (totalNumberOfElements / Settings.ElementsPerPage) + 1;
            return new PagingModel()
            {
                Elements = GeneratePagingElements(url, pageNumber, totalNumberOfElements),
                Last = new PageElement()
                {
                    IsActive = false,
                    Text = Labels.NextPagingElementText,
                    Url = url.AppendParameter(Labels.PagingKey, lastElement.ToString())
                },
                First = new PageElement()
                {
                    IsActive = false,
                    Text = Labels.PreviousPagingElementText,
                    Url = url.AppendParameter(Labels.PagingKey, "1")
                }
            };
        }

        /// <summary>
        /// Genreates a string output of the current shown results related to all results
        /// </summary>
        /// <param name="page">current Page</param>
        /// <param name="totalNumberOfResults">Total Number Of Results</param>
        /// <returns>formated string with the information about the number of results</returns>
        public string BuildResultOutputForCurrentPage(int page, int totalNumberOfResults)
        {
            //Build the result Count Output
            int startNumber = (page - 1) * Settings.ElementsPerPage + 1;
            int endNumber = (page - 1) * Settings.ElementsPerPage + Settings.ElementsPerPage;
            return string.Format(
                Labels.NumberOfResultFormat,
                startNumber,
                endNumber > totalNumberOfResults ? totalNumberOfResults : endNumber,
                totalNumberOfResults);
        }

        #endregion

        #region Helper

        /// <summary>
        /// Helper to generate a specific paging element with the given parameters
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="pageNumber">page number</param>
        /// <param name="totalNumberOfElements">total number of elements</param>
        /// <returns>a filled paging element</returns>
        private static IEnumerable<PageElement> GeneratePagingElements(Uri url, int pageNumber, int totalNumberOfElements)
        {
            // Prepare the limits for the Paging elements
            int numberOfPagingElements = Settings.NumberOfPaingElements;
            int startIndex = pageNumber - numberOfPagingElements > 1 ? pageNumber - numberOfPagingElements : 1;
            int endIndex = pageNumber + numberOfPagingElements <= ((totalNumberOfElements / Settings.ElementsPerPage) + 1)
                ? pageNumber + numberOfPagingElements
                : (totalNumberOfElements / Settings.ElementsPerPage) + 1;

            // Use the limits for building the Paging 
            for (int index = startIndex; index <= endIndex; index++)
            {
                string newUrl = url.AppendParameter(Labels.PagingKey, index.ToString());

                yield return new PageElement()
                {
                    Text = index.ToString(),
                    Url = newUrl,
                    IsActive = pageNumber == index
                };
            }
        }


        #endregion

        /// <summary>
        /// Static Labels
        /// </summary>
        public static class Labels
        {
            /// <summary>
            /// The Paging Key in Url
            /// </summary>
            public const string PagingKey = "page";

            /// <summary>
            /// Text of Previous Paging Element
            /// </summary>
            public const string PreviousPagingElementText = @"&laquo";

            /// <summary>
            /// Text of Next Paging Element
            /// </summary>
            public const string NextPagingElementText = @"&raquo";

            /// <summary>
            /// The base format of the number of results shown
            /// </summary>
            public const string NumberOfResultFormat = "Results {0} - {1} of {2} shown";
        }
    }
}
