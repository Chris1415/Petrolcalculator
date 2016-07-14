using Petrolcalculator.Core.Applications.Services.Web;
using Petrolcalculator.Core.Applications.Services.Web.Implementations;

namespace Petrolcalculator.Core.Applications.Services.Facade.Implementation
{
    /// <summary>
    /// Services to handle Web Stuff
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class WebServiceFacade : IWebServiceFacade
    {
        #region Properties

        /// <summary>
        /// Paging Service 
        /// </summary>
        public IPagingService PagingService { get; }

        #endregion

        #region c'tor

        /// <summary>
        /// c'tor with default implementations
        /// </summary>
        public WebServiceFacade()
        {
            PagingService = new PagingService();
        }

        /// <summary>
        /// c'tor with Paramters
        /// Used in DI e.g.
        /// </summary>
        /// <param name="pagingService">Paging Service</param>
        public WebServiceFacade(IPagingService pagingService)
        {
            PagingService = pagingService;
        }

        #endregion
    }
}
