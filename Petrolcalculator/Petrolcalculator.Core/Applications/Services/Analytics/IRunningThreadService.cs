using System;
using System.Threading;

namespace Petrolcalculator.Core.Applications.Services.Analytics
{
    /// <summary>
    /// Service to access the Running Thread Repository, which tracks all running data collection threads
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IRunningThreadService
    {
        /// <summary>
        /// Add Function
        /// </summary>
        /// <param name="dataCollectionThread">The Thread to be tracked</param>
        /// <returns>Guid to keep track of the thread</returns>
        Guid AddNewDataCollectionThread(Thread dataCollectionThread);

        /// <summary>
        /// Add Function
        /// </summary>
        /// <param name="dataCollectionThread">The Thread to be tracked</param>
        /// <param name="guid">predefnied Guid of the Thread</param>
        void AddNewDataCollectionThread(Thread dataCollectionThread, Guid guid);

        /// <summary>
        ///  Remove Function
        /// </summary>
        /// <param name="guid">Guid of the thread to be removed</param>
        void RemoveDataCollectionThread(Guid guid);

        /// <summary>
        /// Determines if a thread should be shutdowned
        /// </summary>
        /// <param name="guid">guid of the thread</param>
        /// <returns>true if it should be shutdowned</returns>
        bool ShutDownForced(Guid guid);

        /// <summary>
        /// Force a safe shutdonw of a specific thread
        /// </summary>
        /// <param name="guid">guid of the thread</param>
        void ForceShutDown(Guid guid);

    }
}
