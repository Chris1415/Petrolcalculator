using Petrolcalculator.Core.Applications.Services.Web;

namespace Petrolcalculator.Core.Applications.Services.Facade
{
    /// <summary>
    /// Services to handle Web Stuff like in ASP.NET Webforms, MVC ...
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public interface IWebServiceFacade
    {
        #region Properties
        /// <summary>
        /// Paging Service 
        /// </summary>
        IPagingService PagingService { get; }

        #endregion

    }
}
