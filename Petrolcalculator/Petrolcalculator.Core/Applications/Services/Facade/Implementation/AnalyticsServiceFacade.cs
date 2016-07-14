using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using Petrolcalculator.Core.Applications.Helper;
using Petrolcalculator.Core.Applications.Models.Statics;
using Petrolcalculator.Core.Applications.Services.Analytics;
using Petrolcalculator.Core.Applications.Services.Analytics.Implementations;

namespace Petrolcalculator.Core.Applications.Services.Facade.Implementation
{
    /// <summary>
    /// Analytics Service Facade for accessing Analytics Services 
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class AnalyticsServiceFacade : IAnalyticsServiceFacade
    {
        #region Properties

        /// <summary>
        /// Cheaptes Price Service
        /// </summary>
        public IBestPriceAnalyticsService BestPriceAnalyticsService { get; }

        /// <summary>
        /// NLog
        /// </summary>
        private static readonly Logger Logger = Settings.Logging ? LogManager.GetCurrentClassLogger() : LogManager.CreateNullLogger();

        #endregion

        #region c'tor

        /// <summary>
        /// c'tor with parameters 
        /// </summary>
        /// <param name="bestPriceAnalyticsService">Instance of the Cheapest Price Service</param>
        public AnalyticsServiceFacade(IBestPriceAnalyticsService bestPriceAnalyticsService)
        {
            BestPriceAnalyticsService = bestPriceAnalyticsService;
        }

        /// <summary>
        /// Standard c'tor with default initializations
        /// </summary>
        public AnalyticsServiceFacade()
        {
            BestPriceAnalyticsService = new BestPriceAnalyticsEvaluationService();
        }

        #endregion

        #region Interface

        /// <summary>
        /// Evaluate all Analytics Data with all existing analytics services
        /// </summary>
        /// <param name="analyticsData">given analytics data</param>
        /// <returns>Dictionary with Key: Analytics Service Name, Value: Result to be printed in frontend</returns>
        public Dictionary<string, string> EvaluateAllAnalytics(IEnumerable<PetrolStationAnalyticsModel> analyticsData)
        {
            Dictionary<string, string> analyticsDictionary = new Dictionary<string, string>();

            // Go through all Properties(Services) in the Facade and call for each the Evaluate method, which evaluates very specific analytics values
            foreach (PropertyInfo property in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Get the implementation of the given interface
                var givenPropertyType = property.PropertyType;
                var propertyImplementation = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => givenPropertyType.IsAssignableFrom(p))
                    .FirstOrDefault(p => p.IsClass);

                // Get the common Evaluate Method
                MethodInfo evaluateMethod = propertyImplementation?.GetMethod("Evaluate",BindingFlags.Public | BindingFlags.Instance);
                if (evaluateMethod == null)
                {
                    Logger.Error("EvaluateAllAnalytics: Evaluate Method is null");
                    continue;
                }

                // Get the specific value of the property
                object instanceOfProperty = property.GetValue(this);
                // Call the Evaluate method with the common parameter and get the string result
                object returnValue = evaluateMethod.Invoke(instanceOfProperty, new object[] {analyticsData});
                //Add the Result to the dictionary key is the specific Analytics service, value is the result string of the service
                analyticsDictionary.Add(propertyImplementation.ToString(), returnValue.ToString());
            }

            return analyticsDictionary;
        }

        #endregion
    }
}
