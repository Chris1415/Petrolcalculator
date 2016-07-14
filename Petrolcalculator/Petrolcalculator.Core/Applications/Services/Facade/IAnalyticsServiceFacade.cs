using System.Collections.Generic;
using Petrolcalculator.Core.Applications.Models.Statics;
using Petrolcalculator.Core.Applications.Services.Analytics;

namespace Petrolcalculator.Core.Applications.Services.Facade
{
    /// <summary>
    /// Analytics Service Facade for accessing Analytics Services 
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IAnalyticsServiceFacade
    {
        #region Properties

        /// <summary>
        /// Cheaptes Price Service
        /// </summary>
        IBestPriceAnalyticsService BestPriceAnalyticsService { get; }

        #endregion

        #region Interface

        /// <summary>
        /// Evaluate all Analytics Data with all existing analytics services
        /// </summary>
        /// <param name="analyticsData">given analytics data</param>
        /// <returns>Dictionary with Key: Analytics Service Name, Value: Result to be printed in frontend</returns>
        Dictionary<string, string> EvaluateAllAnalytics(IEnumerable<PetrolStationAnalyticsModel> analyticsData);

        #endregion
    }
}
