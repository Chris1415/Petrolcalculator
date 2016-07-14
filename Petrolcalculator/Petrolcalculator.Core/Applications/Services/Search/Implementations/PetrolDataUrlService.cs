using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Petrolcalculator.Core.Applications.Attributes;
using Petrolcalculator.Core.Applications.Options.Base;

namespace Petrolcalculator.Core.Applications.Services.Search.Implementations
{
    /// <summary>
    /// The Petrol Data Url Service
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PetrolDataUrlService : IPetrolDataUrlService
    {
        #region Interface

        /// <summary>
        /// Creates the Petrol Data Service URL based on the given options
        /// </summary>
        /// <typeparam name="T">options type</typeparam>
        /// <param name="baseUrl">base URL</param>
        /// <param name="options">given options</param>
        /// <returns>Response in JSON format</returns>
        public string CreatePetrolDataServiceUrl<T>(string baseUrl, T options) where T : IPetrolDataOptions
        {
            // Build the base url
            string url = $"{baseUrl}?";
            // Get all parameters of the Url with the given options
            GetAllKeyValuePairs(options, ref url);
            return url;
        }

        #endregion

        #region Helper

        /// <summary>
        /// Extracts all Keys and Values for the Parameters of the request Url from the given Options via Attributes
        /// </summary>
        /// <typeparam name="T">Options Type</typeparam>
        /// <param name="options">given options</param>
        /// <param name="url">Url for the request</param>
        public void GetAllKeyValuePairs<T>(T options, ref string url)
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                ParameterName parameterName = property.GetCustomAttribute<ParameterName>();

                if (parameterName == null)
                {
                    // Call this function for a value of the options which is no primitive
                    AppendUrlFromRecursiveGenericCall(property.PropertyType, property.GetValue(options), ref url);
                    continue;
                }

                // Get Key and Value for URL Parameter
                string key = parameterName.Name;
                object value = property.GetValue(options);

                // Value can be a string or a list
                if (value is string)
                {
                    // If Value is a string get it and give it toLower back
                    value = value.ToString().ToLower();
                }
                else if (value is IEnumerable)
                {
                    // If Value is a list get all elements and put them as Komma seperated values one after another
                    string listElements = string.Empty;
                    foreach (var element in value as IEnumerable)
                    {
                        if (!listElements.Equals(string.Empty))
                        {
                            listElements += ",";
                        }

                        listElements += $"\"{element.ToString().ToLower()}\"";
                    }

                    value = $"[{listElements}]";
                }
                else
                {
                    // Default handling get value
                    value = value.ToString().ToLower();
                }

                // Append the url with the Key and built value 
                url += $"{key}={value}&";
            }


            // Examine all Base Interfacetyps
            Type[] baseInterfaces = typeof(T).GetInterfaces();

            if (!baseInterfaces.Any())
            {
                return;
            }

            foreach (Type baseInterfaceType in baseInterfaces)
            {
                // Call this function with each Base Interface
                AppendUrlFromRecursiveGenericCall(baseInterfaceType, options, ref url);
            }
        }
        
        /// <summary>
        /// Helper to Make a recursive generic call with the given type and value to append the url
        /// </summary>
        /// <typeparam name="T">given Type of value</typeparam>
        /// <param name="type">type of input</param>
        /// <param name="options">the value</param>
        /// <param name="url">url to append</param>
        private void AppendUrlFromRecursiveGenericCall<T>(Type type, T options, ref string url)
        {
            MethodInfo method = typeof(PetrolDataUrlService).GetMethod(Labels.MethodForRecursiveReflectionCall);
            MethodInfo generic = method.MakeGenericMethod(type);
            object[] parameters = { options, url };
            generic.Invoke(this, parameters);
            url = (string)parameters[1];
        }

        #endregion

        /// <summary>
        /// Labels class for const strings
        /// </summary>
        public class Labels
        {
            /// <summary>
            /// The Name of the Method, which is called recursivly via reflection
            /// </summary>
            public const string MethodForRecursiveReflectionCall = "GetAllKeyValuePairs";
        }
    }
}
