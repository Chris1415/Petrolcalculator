using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Petrolcalculator.Core.Applications.Models.Json.PriceRequests.Implementation;
using Petrolcalculator.Core.Applications.Models.Statics;

namespace Petrolcalculator.Core.Applications.Services.Analytics.Implementations
{
    /// <summary>
    /// Analytics Evaluation Service for evaluationg the collected analytics data
    /// Get the Cheapest Price for petrol regarding the given data
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class BestPriceAnalyticsEvaluationService : IBestPriceAnalyticsService
    {
        #region Interface 

        /// <summary>
        ///  Evaluationmethod to use a list of analytics data to calculate the period of time where the petrol is cheapest
        /// </summary>
        /// <param name="analyticsData">the analytics data</param>
        /// <returns>a string to be printed in frontet, with all information about the results</returns>
        public string Evaluate(IEnumerable<PetrolStationAnalyticsModel> analyticsData)
        {
            // Get information of the case: (One Station / Multiple Stations) and (One Day / Multiple Days)
            bool singlePetrolStation = analyticsData
                .Select(element => element.PetrolStationId)
                .Distinct()
                .Count() == 1;
            bool singleDay = analyticsData
                .Select(element => element.RequestDatetime.ToString(Labels.Format.DayMonthFormat))
                .Distinct()
                .Count() == 1;

            // Based on the result call the right calclulation
            // Only single PEtrol station and single Day is supported
            return singleDay && singlePetrolStation
                ? EvaluateForSingleStationSingleDay(analyticsData)
                : Labels.Text.NotSupported;
        }

        #endregion

        #region Helper

        #region Single Day

        /// <summary>
        /// Calculate the Time Period(s) where the petrol is the cheapest
        /// </summary>
        /// <param name="analyticsData">given analytics data</param>
        /// <returns>string to print in frontend with the result</returns>
        private static string EvaluateForSingleStationSingleDay(IEnumerable<PetrolStationAnalyticsModel> analyticsData)
        {
            // Get all Time Periods of minimum price
            Tuple<double, IEnumerable<IList<DateTime>>> times = GenerateTimesListForSingleDay(analyticsData);
            // Build the resultstring with the given information
            string resultString = BuildResultStringWithTimesOnSingleDay(times.Item2);
            return resultString;
        }

        /// <summary>
        /// Take the analytics list for a single day and a single petrol station and calculate the periods of times with the best price
        /// </summary>
        /// <param name="analyticsData">given analytics </param>
        /// <returns>Tuple with the minimum price and the times</returns>
        private static Tuple<double, IEnumerable<IList<DateTime>>> GenerateTimesListForSingleDay(IEnumerable<PetrolStationAnalyticsModel> analyticsData)
        {
            IList<IList<DateTime>> times = new List<IList<DateTime>>();
            double minimumPrice = double.MaxValue;

            // Go through all analytics data
            foreach (PetrolStationAnalyticsModel data in analyticsData)
            {
                // Get the price
                double price = GetPriceForCaluclation(data);
                // Check the current price with the minimum one
                if (price < minimumPrice)
                {
                    //If it is lower, save the new price
                    minimumPrice = price;
                    // Clear all saved data
                    times.Clear();
                    // Insert the new element into the empty list
                    times.Add(new List<DateTime>()
                    {
                        data.RequestDatetime
                    });
                }
                else if (Math.Abs(price - minimumPrice) < 0.001)
                {
                    // If it is "equal" insert this element into the list of minimum price times
                    times.ElementAt(times.Count - 1).Add(data.RequestDatetime);
                }
                else
                {
                    // If the current price is higher, check if the current List of minimum elements has elements
                    if (times.Last().Any())
                    {
                        // If elements are present, the current time period is over, add a new list for new minimum price times
                        times.Add(new List<DateTime>());
                    }
                }
            }

            // Check if the last period has elements saved -> If nont delete the last period
            if (!times.Last().Any())
            {
                times.RemoveAt(times.Count - 1);
            }

            // Rreturn the new Tuple
            return new Tuple<double, IEnumerable<IList<DateTime>>>(minimumPrice, times);
        }

        /// <summary>
        /// Get Price in a safe way for further calculations
        /// </summary>
        /// <param name="data">specific data</param>
        /// <returns>price in double</returns>
        private static double GetPriceForCaluclation(PetrolDataServicePriceResultModel data)
        {
            // Get a valid price from E5, as long as no filtering by petrol type is activated
            // E5 is the petrol type, which is available at nearly every petrol station
            double price = double.TryParse(data.E5, NumberStyles.Any , CultureInfo.InvariantCulture, out price)
                ? price
                : double.MaxValue;

            // Check if the price is close to 0 -> is 0, petrol type not in petrol station
            return Math.Abs(price) < 0.001 ? double.MaxValue : price;
        }

        /// <summary>
        /// Builds the result as string based on the given time periods
        /// </summary>
        /// <param name="times">given time periods</param>
        /// <returns>string to print in frontend with the result</returns>
        private static string BuildResultStringWithTimesOnSingleDay(IEnumerable<IList<DateTime>> times)
        {
            string builtResult = (
                from timePeriod
                    in times
                let firstTime = timePeriod.FirstOrDefault()
                let lastTime = timePeriod.LastOrDefault()
                select
                    $"({firstTime.ToString(Labels.Format.HourMinuteFormat)} - {lastTime.ToString(Labels.Format.HourMinuteFormat)}) ")
                    .Aggregate(Labels.Text.SingleDayBaseString,
                        (current, period) => current + period);

            return builtResult;
        }

        #endregion    

        #endregion

        /// <summary>
        /// Static Labels
        /// </summary>
        public static class Labels
        {
            /// <summary>
            /// Texte
            /// </summary>
            public static class Text
            {
                /// <summary>
                /// Error Text if no Petrol Station is chosen
                /// </summary>
                public const string NotSupported = "Choose a specific Petrol Station and Day for Best Price Analytics";

                /// <summary>
                /// Bese result text for single day analytics
                /// </summary>
                public const string SingleDayBaseString = "Best price for refuel at chosen day is at: ";
            }

            /// <summary>
            /// Formtat strings
            /// </summary>
            public static class Format
            {
                /// <summary>
                /// Date Time format for output Day Month
                /// </summary>
                public const string DayMonthFormat = "dd MMMM";

                /// <summary>
                /// Date Time formtat four output Hour:Minute
                /// </summary>
                public const string HourMinuteFormat = "HH:mm";
            }
        }
    }
}
