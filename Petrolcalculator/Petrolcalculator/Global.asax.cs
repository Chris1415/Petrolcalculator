using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Petrolcalculator.Core.Applications.Helper;
using Petrolcalculator.Core.Applications.Models.Statics;
using Petrolcalculator.Core.Applications.Repositories.Implementation;
using Petrolcalculator.Core.Applications.Services.Facade;
using Petrolcalculator.Core.Applications.Services.Facade.Implementation;

namespace Petrolcalculator
{
    /// <summary>
    /// Global Asax
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        #region Properties
        /// <summary>
        /// Service Facade
        /// </summary>
        public readonly IPetrolDataServiceFacade PetrolServiceFacade = new PetrolDataServiceFacade();

        #endregion

        /// <summary>
        /// Application Start
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Get the Analytics Data Model at App Start
            PetrolStationAnalyticsRepository.AnalyticsEntries = PetrolServiceFacade
                .SerializationService
                .Deserialize<List<PetrolStationAnalyticsModel>>(Settings.AnaylticsSerializationPath);

        }
    }
}
