using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NLog;
using Petrolcalculator.Core.Applications.Helper;

namespace Petrolcalculator.Core.Applications.Services.Json.Implementations
{
    /// <summary>
    /// Service to handle JSON Serialization of classes
    /// </summary>
    /// <author>Christian Hahn, Jun-2016
    /// </author>
    public class JsonSerializationService : IJsonSerializationService
    {
        #region Properties
        /// <summary>
        /// The Binary Formater for Json Serialization/Deserialization
        /// </summary>
        private readonly BinaryFormatter Formatter = new BinaryFormatter();

        /// <summary>
        /// NLog
        /// </summary>
        private static readonly Logger Logger = Settings.Logging ? LogManager.GetCurrentClassLogger() : LogManager.CreateNullLogger();

        #endregion

        #region Interface

        /// <summary>
        /// Serializes a given Model and saves the result to the given filepath
        /// </summary>
        /// <typeparam name="T">modeltype</typeparam>
        /// <param name="model">given model</param>
        /// <param name="filePath">giiven file path</param>
        public void Serialize<T>(T model, string filePath)
        {
            try
            {
                // Create the Filestream
                FileStream stream = File.Create(filePath);
                // Serialize
                Formatter.Serialize(stream, model);
                // Close the Filestream
                stream.Close();
            }
            catch (Exception e)
            {
                Logger.Error("Serialize: " + e.Message);
            }           
        }

        /// <summary>
        /// Deserializes a model from a given filepath
        /// </summary>
        /// <typeparam name="T">modeltype</typeparam>
        /// <param name="filePath">given filepath</param>
        /// <returns>instance of the deserialized model</returns>
        public T Deserialize<T>(string filePath) where T : new()
        {
            try
            {
                // Create the Filestream
                FileStream input = File.Open(filePath, FileMode.Open);
                // Deserialize
                return (T) Formatter.Deserialize(input);
            }
            catch (Exception e)
            {
                Logger.Error("Deserialize: " + e.Message);
                return new T();
            }
        }

        #endregion
    }
}
