using Petrolcalculator.Core.Applications.Options;

namespace Petrolcalculator.Core.Applications.Services.Search
{
    /// <summary>
    /// Service to check if specific requests are restricted
    /// Restriction is -> Time Violation
    /// Every identical call may only requested in a timespan greater X Min
    /// Every Call with identical options in a timespan less than X Min should be restricted
    /// X is adjustable in the web.config under "DelayBetweenTwoIdenticalRequests"
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IRestrictedPetrolDataRequestService
    {
        /// <summary>
        /// Check a spefici Request with options for Violation of restriction
        /// If it violates the restriction 
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="options"></param>
        /// <returns>The Results for a saved request</returns>
        T CheckRequest<T>(IPetrolDataListOptions options) where T : new();

        /// <summary>
        /// Adds a Request with Result to the History Repository
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="options">Options for the request</param>
        /// <param name="results">Results for the request</param>
        void AddRequestWithResult<T>(IPetrolDataListOptions options, T results) where T : new();
    }
}
