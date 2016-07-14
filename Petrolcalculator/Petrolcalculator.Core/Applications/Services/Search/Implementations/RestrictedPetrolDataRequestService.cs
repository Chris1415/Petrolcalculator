using System;
using Petrolcalculator.Core.Applications.Helper;
using Petrolcalculator.Core.Applications.Models;
using Petrolcalculator.Core.Applications.Options;
using Petrolcalculator.Core.Applications.Repositories.Implementation;

namespace Petrolcalculator.Core.Applications.Services.Search.Implementations
{
    /// <summary>
    /// Service to check if specific requests are restricted
    /// Restriction is -> Time Violation
    /// Every identical call may only requested in a timespan greater 4 Min
    /// Every Call with identical options in a timespan less than 4 Min should be restricted
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class RestrictedPetrolDataRequestService : IRestrictedPetrolDataRequestService
    {
        #region Interface
        /// <summary>
        /// Check a spefici Request with options for Violation of restriction
        /// If it violates the restriction 
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="options"></param>
        /// <returns>The Results for a saved request</returns>
        public T CheckRequest<T>(IPetrolDataListOptions options) where T : new()
        {
            // Go through all Elements in the Repository
            for (int index = 0; index < RequestHistoryRepository<T>.HistoryRequests.Count; index++)
            {
                RequestHistoryObject<T> requestObject = RequestHistoryRepository<T>.HistoryRequests[index];
                TimeSpan goneTime = DateTime.Now - requestObject.RequestTime;
                // Delete all elements, where the request is longer than X in the history
                if (MoreThanXMinutesElapsed(goneTime, Settings.DelayBetweenTwoIdenticalRequests))
                {
                    RequestHistoryRepository<T>.HistoryRequests.RemoveAt(index);
                    continue;
                }

                // Check if the current options are equal to a saved request
                if (requestObject.RequestOptions.Equals(options))
                {
                    // If a request with the same options is in the list, give the cached results back
                    return requestObject.RequestResult;
                }
            }

            // If nothing is found give the default value back -> null
            return default(T);
        }

        /// <summary>
        /// Adds a Request with Result to the History Repository
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="options">Options for the request</param>
        /// <param name="results">Results for the request</param>
        public void AddRequestWithResult<T>(IPetrolDataListOptions options, T results) where T : new()
        {
            // Add a new Entry to the Repository of cached results based on the given options and the current request time
            RequestHistoryRepository<T>.HistoryRequests.Add(new RequestHistoryObject<T>()
            {
                RequestOptions = options,
                RequestResult = results,
                RequestTime = DateTime.Now
            });
        }

        #endregion 

        #region Helper

        /// <summary>
        /// Helper to determine if a ts is greater than X minutes 
        /// X less or equals than 60
        /// </summary>
        /// <param name="ts">time span</param>
        /// <param name="thresholdInMinutes">threshold in minutes</param>
        /// <returns>true if the time span is greater than the threshold</returns>
        private static bool MoreThanXMinutesElapsed(TimeSpan ts, int thresholdInMinutes)
        {
            // Check if the minutes are greater than the threshold
            if (ts.Minutes >= thresholdInMinutes)
            {
                return true;
            }

            // Check the case if the minutes are less than the threshold, but the hours or days are greater 0
            return ts.Hours > 0 || ts.Days > 0;
        }

        #endregion
    }
}
