using System;
using Newtonsoft.Json;
using NLog;
using Petrolcalculator.Core.Applications.Helper;

namespace Petrolcalculator.Core.Applications.Services.Json.Implementations
{
    /// <summary>
    /// Generic Json to Class Mapper
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class JsonMapperService : IJsonMapperService
    {
        #region Properties
        
        /// <summary>
        /// NLog
        /// </summary>
        private static readonly Logger Logger = Settings.Logging ? LogManager.GetCurrentClassLogger() : LogManager.CreateNullLogger();

        #endregion

        #region Interface

        /// <summary>
        /// Maps the given string in JSON format to the given class
        /// </summary>
        /// <typeparam name="T">given class type</typeparam>
        /// <param name="json">input json</param>
        /// <returns>instance of the created class</returns>
        public T MapJsonToClass<T>(string json) where T : new()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Logger.Info("MapJsonToClass: " + e.Message);
                return new T();
            }           
        }

        #endregion
    }
}
