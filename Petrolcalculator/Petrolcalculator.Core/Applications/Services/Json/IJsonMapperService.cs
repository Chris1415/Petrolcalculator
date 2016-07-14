namespace Petrolcalculator.Core.Applications.Services.Json
{
    /// <summary>
    /// Generic Json to Class Mapper
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IJsonMapperService
    {
        /// <summary>
        /// Maps the given string in JSON format to the given class
        /// </summary>
        /// <typeparam name="T">given class type</typeparam>
        /// <param name="json">input json</param>
        /// <returns>instance of the created class</returns>
        T MapJsonToClass<T>(string json) where T : new();
    }
}
