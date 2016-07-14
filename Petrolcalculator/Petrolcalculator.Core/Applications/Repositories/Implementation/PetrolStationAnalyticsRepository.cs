using System.Collections.Generic;
using Petrolcalculator.Core.Applications.Models.Statics;

namespace Petrolcalculator.Core.Applications.Repositories.Implementation
{
    /// <summary>
    /// The Repository for storing all collected analytics data
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public static class PetrolStationAnalyticsRepository
    {
        /// <summary>
        /// List of analytics data
        /// No Dictionary but List was chosen for easy Serialization / Deserialization
        /// </summary>
        public static IList<PetrolStationAnalyticsModel> AnalyticsEntries = new List<PetrolStationAnalyticsModel>();
    }
}
