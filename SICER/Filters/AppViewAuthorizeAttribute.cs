using SICER.HELPER;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace SICER.Filters
{
    public class AppViewAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {

        private readonly String[] _views;
        private readonly Int32 _idTurnoCoordinador;

        public AppViewAuthorizeAttribute(params String[] views)
        {
            _views = views;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var unauthorized = false;

            try
            {
                var views = filterContext.HttpContext.Session.GetPermisosVista();

                var intersect = views.Intersect(_views);
                if (intersect.Count() == 0)
                    unauthorized = true;
            }
            catch (Exception)
            {
                unauthorized = true;
            }

            if (unauthorized)
            {
                if (filterContext.HttpContext.Session.GetIdUsuario() == null)
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login", area = "" }));

                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PermisoInsuficiente", area = "" }));
                }
            }
        }

    }
}