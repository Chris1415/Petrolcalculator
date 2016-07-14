using System.Collections.Generic;
using Petrolcalculator.Core.Applications.Models;

namespace Petrolcalculator.Core.Applications.Repositories.Implementation
{
    /// <summary>
    /// Repository to store Requests and their Results
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public static class RequestHistoryRepository<T>
    {
        /// <summary>
        /// The List for storing every single request object
        /// </summary>
        public static IList<RequestHistoryObject<T>> HistoryRequests = new List<RequestHistoryObject<T>>();
    }
}
