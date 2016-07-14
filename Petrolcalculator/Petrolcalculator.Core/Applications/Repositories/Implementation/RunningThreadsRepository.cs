using System;
using System.Collections.Generic;
using Petrolcalculator.Core.Applications.Models;

namespace Petrolcalculator.Core.Applications.Repositories.Implementation
{
    /// <summary>
    /// Static Data Structure to hold information about running Data Collection Threads
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public static class RunningThreadsRepository
    {
        /// <summary>
        /// Dictionary to map a Thread to a specific guid
        /// </summary>
        public static Dictionary<Guid, ThreadWithLivingStatus> RunningThreads = new Dictionary<Guid, ThreadWithLivingStatus>();
    }
}
