using System;
using Petrolcalculator.Core.Applications.Options;

namespace Petrolcalculator.Core.Applications.Models
{
    /// <summary>
    /// The Request History Object
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class RequestHistoryObject<T>
    {
        /// <summary>
        /// Request Time
        /// </summary>
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// Request Options
        /// </summary>
        public IPetrolDataListOptions RequestOptions { get; set; }

        /// <summary>
        /// Request Results
        /// </summary>
        public T RequestResult { get; set; }
    }
}
