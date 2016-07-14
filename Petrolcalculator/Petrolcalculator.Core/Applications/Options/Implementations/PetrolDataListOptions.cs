using Petrolcalculator.Core.Applications.Helper;
using Petrolcalculator.Core.Applications.Models;
using Petrolcalculator.Core.Applications.Models.Enums;

namespace Petrolcalculator.Core.Applications.Options.Implementations
{
    /// <summary>
    /// Petrol Data Service Options for List Requests
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataListOptions : IPetrolDataListOptions
    {
        #region c'tor

        /// <summary>
        /// Default c'tor
        /// </summary>
        public PetrolDataListOptions()
        {
            ApiKey = Settings.PetrolKingApiKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The Geoposition
        /// </summary>
        public Geoobject GeoPosition { get; set; }

        /// <summary>
        /// The Radius
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// The Sort Order of the Results
        /// </summary>
        public SortOrder SortOrder { get; set; }

        /// <summary>
        /// The used Petrol Type
        /// </summary>
        public PetrolTypes PetrolType { get; set; }

        /// <summary>
        /// The APi Key
        /// </summary>
        public string ApiKey { get; }

        #endregion

        #region Helper

        /// <summary>
        /// Helper to check if the Model is valid for a Request
        /// </summary>
        /// <returns>true if the model is valid</returns>
        public bool IsValid => GeoPosition.IsValid
                               && Radius >= 0
                               && Radius <= 25
                               && !ApiKey.IsNullOrEmpty();


        #endregion

        /// <summary>
        /// Equals Overload to determine, when two options are the same
        /// </summary>
        /// <param name="other">Second Options</param>
        /// <returns>true, if the options are the same, otherwise false</returns>
        public bool Equals(IPetrolDataListOptions other)
        {
            return this.SortOrder.Equals(other.SortOrder)
                   && this.GeoPosition.Equals(other.GeoPosition)
                   && this.PetrolType.Equals(other.PetrolType)
                   && this.Radius.Equals(other.Radius);
        }
    }
}
