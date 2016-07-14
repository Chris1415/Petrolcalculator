using System;
using Petrolcalculator.Core.Applications.Attributes;
using Petrolcalculator.Core.Applications.Helper;

namespace Petrolcalculator.Core.Applications.Models
{
    /// <summary>
    /// Geoobject Class
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class Geoobject : IEquatable<Geoobject>
    {
        #region c'tor


        /// <summary>
        /// c'tor default
        /// Invalid Geodata
        /// </summary>
        public Geoobject()
        {
            Lat = int.MinValue;
            Lng = int.MinValue;
        }

        /// <summary>
        /// c'tor with double input
        /// </summary>
        /// <param name="lat">latitude in string</param>
        /// <param name="lng">longtitude in string</param>
        public Geoobject(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }

        /// <summary>
        /// c'tor with string input
        /// </summary>
        /// <param name="lat">latitude in string</param>
        /// <param name="lng">longtitude in string</param>
        public Geoobject(string lat, string lng)
        {
            double latDouble;
            double lngDouble;

            if (lat.IsNullOrEmpty()
                || lng.IsNullOrEmpty()
                || !double.TryParse(lat.Replace(".", ","), out latDouble)
                || !double.TryParse(lng.Replace(".", ","), out lngDouble))
            {
                Lat = int.MinValue;
                Lng = int.MinValue;
                return;
            }

            Lat = latDouble;
            Lng = lngDouble;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Latitude
        /// </summary>
        [ParameterName("lat")]
        public double Lat { get; set; }

        /// <summary>
        /// Longtitude
        /// </summary>
        [ParameterName("lng")]
        public double Lng { get; set; }

        #endregion Properties

        #region Helper

        /// <summary>
        /// Validates the given Model
        /// </summary>
        public bool IsValid => Lat >= -90
            && Lat <= 90
            && Lng >= -180
            && Lng <= 180;

        #endregion

        /// <summary>
        /// Equality between geolocations
        /// </summary>
        /// <param name="other">other geolocation</param>
        /// <returns>true if the geolocation has the same lat and lng, otherwise false</returns>
        public bool Equals(Geoobject other)
        {
            return this.Lat.Equals(other.Lat)
                   && this.Lng.Equals(other.Lng);
        }
    }
}
