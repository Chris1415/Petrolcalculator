using System.Threading;

namespace Petrolcalculator.Core.Applications.Models
{
    /// <summary>
    /// Extension of a normal Thread with the Property, with which the Thread can be shut down safely
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class ThreadWithLivingStatus
    {
        /// <summary>
        /// Flag to determine if the current Thread should be shut downed
        /// </summary>
        public bool ShutDownForced { get; set; }

        /// <summary>
        /// Instance of a Thread
        /// </summary>
        public Thread CurrentThread { get; set; }
    }
}
