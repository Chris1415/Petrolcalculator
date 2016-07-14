namespace Petrolcalculator.Core.Applications.Models.Web
{
    /// <summary>
    /// Model to store information about current Pagging
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PageElement
    {
        /// <summary>
        /// Text of the Link
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Url of the Link
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Flag to determine of the Link is active right now
        /// </summary>
        public bool IsActive { get; set; }
    }
}