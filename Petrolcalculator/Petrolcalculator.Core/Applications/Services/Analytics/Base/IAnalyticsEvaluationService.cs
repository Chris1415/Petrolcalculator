using System.Collections.Generic;
using Petrolcalculator.Core.Applications.Models.Statics;

namespace Petrolcalculator.Core.Applications.Services.Analytics.Base
{
    /// <summary>
    /// General Analytics Evaluation Service for evaluationg the collected analytics data
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IAnalyticsEvaluationService
    {
        /// <summary>
        /// General Evaluationmethod to use a list of analytics data
        /// </summary>
        /// <param name="analyticsData">the analytics data</param>
        /// <returns>a string to be printed in frontet, with all information about the results</returns>
        string Evaluate(IEnumerable<PetrolStationAnalyticsModel> analyticsData);
    }
}
