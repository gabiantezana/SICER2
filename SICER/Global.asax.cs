using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hangfire;
using Hangfire.SqlServer;
using Owin;
using SICER.Areas.Sync.Controllers;
using SICER.Controllers;
using SICER.DATAACCESS.Sync;
using SICER.EXCEPTION;
using SICER.LOGIC.Sync;
using SICER.MODEL;

namespace SICER
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private BackgroundJobServer _backgroundJobServer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //TODO: VALIDATE DATABASE STRUCTURE
            // BaseController().InitializeApplication();

            GlobalConfiguration.Configuration.UseSqlServerStorage(new SICEREntities().Database.Connection.ConnectionString);
            _backgroundJobServer = new BackgroundJobServer();

            //BackgroundJob.Enqueue(() => DoWork());
        }

        public static void DoWork()
        {
            while (true)
            {
                new SyncController().SyncDataBase();
            }
        }
    }
}

