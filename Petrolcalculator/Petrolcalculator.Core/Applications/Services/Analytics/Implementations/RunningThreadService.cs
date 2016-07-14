using System;
using System.Threading;
using Petrolcalculator.Core.Applications.Models;
using Petrolcalculator.Core.Applications.Repositories.Implementation;

namespace Petrolcalculator.Core.Applications.Services.Analytics.Implementations
{
    /// <summary>
    /// Service to access the Running Thread Repository, which tracks all running data collection threads
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class RunningThreadService : IRunningThreadService
    {
        /// <summary>
        /// Add Function
        /// </summary>
        /// <param name="dataCollectionThread">The Thread to be tracked</param>
        /// <returns>Guid to keep track of the thread</returns>
        public Guid AddNewDataCollectionThread(Thread dataCollectionThread)
        {
            // Create a Guid 
            Guid guid = Guid.NewGuid();
            // Save the current Data collection thread with its created guid
            AddNewDataCollectionThread(dataCollectionThread, guid);
            // Return the guid for observing/killing the thread later
            return guid;
        }

        /// <summary>
        /// Add Function
        /// </summary>
        /// <param name="dataCollectionThread">The Thread to be tracked</param>
        /// <param name="guid">predefnied Guid of the Thread</param>
        public void AddNewDataCollectionThread(Thread dataCollectionThread, Guid guid)
        {
            // Add a new Data Collection thread, where the guid is already created previously
            RunningThreadsRepository.RunningThreads.Add(guid, new ThreadWithLivingStatus()
            {
                ShutDownForced = false,
                CurrentThread = dataCollectionThread
            });
        }

        /// <summary>
        ///  Remove Function
        /// </summary>
        /// <param name="guid">Guid of the thread to be removed</param>
        public void RemoveDataCollectionThread(Guid guid)
        {
            RunningThreadsRepository.RunningThreads.Remove(guid);
        }

        /// <summary>
        /// Determines if a thread should be shutdowned
        /// </summary>
        /// <param name="guid">guid of the thread</param>
        /// <returns>true if it should be shutdowned</returns>
        public bool ShutDownForced(Guid guid)
        {
            // Flag to check if the Call "ForceShutDown" has been made
            ThreadWithLivingStatus dictionaryValue = RunningThreadsRepository.RunningThreads[guid];
            return dictionaryValue.ShutDownForced;
        }

        /// <summary>
        /// Force a safe shutdonw of a specific thread
        /// </summary>
        /// <param name="guid">guid of the thread</param>
        public void ForceShutDown(Guid guid)
        {
            // Set the "ShutDownForced" Flag for a thread with the given Guid
            ThreadWithLivingStatus dictionaryValue = RunningThreadsRepository.RunningThreads[guid];
            dictionaryValue.ShutDownForced = true;
        }
    }
}
