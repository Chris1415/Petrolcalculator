using System.Collections.Generic;
using Petrolcalculator.Core.Applications.Attributes;
using Petrolcalculator.Core.Applications.Options.Base;

namespace Petrolcalculator.Core.Applications.Options
{
    /// <summary>
    /// Petrol Data Service Options for Price Requests
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IPetrolDataPriceOptions : IPetrolDataOptions
    {
        /// <summary>
        /// Specific Petrol Station IDs to call
        /// </summary>
        [ParameterName("ids")]
        IEnumerable<string> Ids { get; set; }
    }
}
