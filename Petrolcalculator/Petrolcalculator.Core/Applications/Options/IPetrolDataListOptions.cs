using System;
using Petrolcalculator.Core.Applications.Attributes;
using Petrolcalculator.Core.Applications.Models;
using Petrolcalculator.Core.Applications.Models.Enums;
using Petrolcalculator.Core.Applications.Options.Base;

namespace Petrolcalculator.Core.Applications.Options
{
    /// <summary>
    /// Petrol Data Service Options for List Requests
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IPetrolDataListOptions : IPetrolDataOptions, IEquatable<IPetrolDataListOptions>
    {
        /// <summary>
        /// The Geoposition
        /// </summary>
        Geoobject GeoPosition { get; set; }

        /// <summary>
        /// The Radius
        /// </summary>
        [ParameterName("rad")]
        double Radius { get; set; }

        /// <summary>
        /// The Sort Order of the Results
        /// </summary>
        [ParameterName("sort")]
        SortOrder SortOrder { get; set; }

        /// <summary>
        /// The used Petrol Type
        /// </summary>
        [ParameterName("type")]
        PetrolTypes PetrolType { get; set; }
    }
}
