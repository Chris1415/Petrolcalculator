using Petrolcalculator.Core.Applications.Models;

namespace Petrolcalculator.Core.Applications.Services.Search
{
    /// <summary>
    /// Service to Map in put like Postcode, street, city to a valid geolocation
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IGeolocationMappingService
    {
        /// <summary>
        /// Takes a string and maps it to a geolocation
        /// </summary>
        /// <returns>a geoobject</returns>
        Geoobject MapInputToGeolocation(string input);
    }
}
