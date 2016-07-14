using System.Collections.Generic;

namespace Petrolcalculator.Core.Applications.Models.Web
{
    /// <summary>
    /// The whole paging model
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class PagingModel
    {
        /// <summary>
        /// Previous Element
        /// </summary>
        public PageElement First { get; set; }

        /// <summary>
        /// All Middle Elements
        /// </summary>
        public IEnumerable<PageElement> Elements { get; set; }
        
        /// <summary>
        /// Next Element
        /// </summary>
        public PageElement Last { get; set; } 
    }
}
