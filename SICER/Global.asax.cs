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
using SICER.DATAACCESS.Sync;
using SICER.EXCEPTION;
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

            //--------------------------------------------------SINCRONIZACIÓN--------------------------------------------------
            //http://docs.hangfire.io/en/latest/users-guide/background-processing/processing-background-jobs.html
            //JobStorage.Current = new SqlServerStorage(new SICEREntities().Database.Connection.ConnectionString);

            //var storage = new SqlServerStorage(System.Configuration.ConfigurationManager.ConnectionStrings["db_HangFire"].ConnectionString); // db_HangFire is the connection string for Sql Server DB used as Job Storage for HangFire for processing 
            //var options = new BackgroundJobServerOptions();

            //var _backgroundJobServer = new BackgroundJobServer(options, storage);
            //_backgroundJobServer.Start(); // start BackgroundJobServer process
            //JobStorage.Current = storage; // assign the storage to Current

            GlobalConfiguration.Configuration.UseSqlServerStorage(new SICEREntities().Database.Connection.ConnectionString);
            _backgroundJobServer = new BackgroundJobServer();


            var dataContext = new DataContext() { Context = new SICEREntities() };
            var dataAccess = new SyncDataAccess(dataContext);

            //BackgroundJob.Enqueue(() => DoWork());

            //--------------------------------------------------SINCRONIZACIÓN--------------------------------------------------


        }
        /*
        public void DoWork()
        {
            var dataContext = new DataContext() { Context = new SICEREntities() };
            var n = 0;
        
        }*/
    }
}

