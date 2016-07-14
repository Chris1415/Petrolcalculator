namespace Petrolcalculator.Models.ViewModels
{
    /// <summary>
    /// The View Model for the Analytics Page
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class AnalyticsPageViewModel
    {
        /// <summary>
        /// Flag to determine if a datacollection thread is already running for the session
        /// </summary>
        public bool IsDataCollectionThread { get; set; }
    }
}