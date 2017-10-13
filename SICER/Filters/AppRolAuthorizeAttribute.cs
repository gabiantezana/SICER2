using SICER.HELPER;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace SICER.Filters
{
    public class AppRolAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly AppRol[] _rol;

        public AppRolAuthorizeAttribute(params AppRol[] rol)
        {
            _rol = rol;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var unauthorized = false;

            try
            {
                var usuario = filterContext.HttpContext.Session["Codigousuario"];
                var rol = filterContext.HttpContext.Session.GetRol();

                if (!_rol.Contains(rol))
                    unauthorized = true;
            }
            catch (Exception)
            {
                unauthorized = true;
            }

            if (unauthorized)
            {
                if (filterContext.HttpContext.Session["Codigousuario"] == null)
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login", area = "" }));
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PermisoInsuficiente", area = "" }));
                }
            }
        }
    }
}