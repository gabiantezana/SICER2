using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SICER.HELPER;

namespace SICER.Filters
{
    public class AppViewAndDocumentTypeAuthorizeAttribute
    {
        private readonly String[] _views;
        private readonly Int32 _idTurnoCoordinador;
        private readonly DocumentView _documentView;

        public AppViewAndDocumentTypeAuthorizeAttribute(DocumentView documentView)
        {
            _documentView = documentView;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var unauthorized = false;

            try
            {
                switch (_documentView)
                {
                    case DocumentView.Listar:
                        break;
                    case DocumentView.Crear:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


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