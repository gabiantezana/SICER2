using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ExpressiveAnnotations.Attributes;
using Hangfire;
using Hangfire.SqlServer;
using Owin;
using SICER.Controllers;
using SICER.DATAACCESS.Sync;
using SICER.EXCEPTION;
using SICER.LOGIC;
using SICER.MODEL;
using SICER.SAPUSERMODEL;
using ExpressiveAnnotations.Attributes;
using ExpressiveAnnotations.MvcUnobtrusive.Providers;
using ExpressiveAnnotations.MvcUnobtrusive.Validators;
namespace SICER
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private BackgroundJobServer _backgroundJobServer;

        protected void Application_Start()
        {
            try
            {

                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);

                //--------------------------------------------------SINCRONIZACIÓN--------------------------------------------------
                GlobalConfiguration.Configuration.UseSqlServerStorage(new SICEREntities().Database.Connection.ConnectionString);
                _backgroundJobServer = new BackgroundJobServer();


                //BackgroundJob.Enqueue(() => DoWork());

                //--------------------------------------------------SINCRONIZACIÓN--------------------------------------------------

                //REQUIREDIF
                DataAnnotationsModelValidatorProvider.RegisterAdapter(
                    typeof(RequiredIfAttribute), typeof(RequiredIfValidator));
                DataAnnotationsModelValidatorProvider.RegisterAdapter(
                    typeof(AssertThatAttribute), typeof(AssertThatValidator));

                ModelValidatorProviders.Providers.Remove(
                    ModelValidatorProviders.Providers
                        .FirstOrDefault(x => x is DataAnnotationsModelValidatorProvider));
                ModelValidatorProviders.Providers.Add(
                    new ExpressiveAnnotationsModelValidatorProvider());
                //
            }
            catch (Exception ex)
            {

            }


        }
    }
}

