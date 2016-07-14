namespace Petrolcalculator.Core.Applications.Services.Base
{
    /// <summary>
    /// Service to handle Serialization of classes  
    /// </summary>
    /// <author>Christian Hahn, Jun-2016
    /// </author>
    public interface ISerializationService
    {
        /// <summary>
        /// Serializes a given Model and saves the result to the given filepath
        /// </summary>
        /// <typeparam name="T">modeltype</typeparam>
        /// <param name="model">given model</param>
        /// <param name="filePath">giiven file path</param>
        void Serialize<T>(T model, string filePath);

        /// <summary>
        /// Deserializes a model from a given filepath
        /// </summary>
        /// <typeparam name="T">modeltype</typeparam>
        /// <param name="filePath">given filepath</param>
        /// <returns>instance of the deserialized model</returns>
        T Deserialize<T>(string filePath) where T : new();
    }
}
