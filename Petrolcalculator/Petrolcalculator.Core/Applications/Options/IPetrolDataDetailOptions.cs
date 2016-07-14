using Petrolcalculator.Core.Applications.Attributes;
using Petrolcalculator.Core.Applications.Options.Base;

namespace Petrolcalculator.Core.Applications.Options
{
    /// <summary>
    /// Petrol Data Service Options for Detail Requests
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IPetrolDataDetailOptions : IPetrolDataOptions
    {
        /// <summary>
        /// Specific Petrol Station IDs to call
        /// </summary>
        [ParameterName("id")]
        string Id { get; set; }
    }
}
