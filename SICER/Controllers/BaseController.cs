using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace SICER.Controllers
{
    [HandleError]
    public class BaseController : Controller
    {
        private DataContext dataContext;
        public SICEREntities context { get; set; }
        public String currentCulture { get; set; }
        protected HttpBrowserCapabilitiesBase Browser { get; set; }

        public BaseController()
        {
            if (context == null)
                context = new SICEREntities();
            currentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
        }

        public DataContext GetDataContext()
        {
            if (dataContext == null)
                dataContext = new DataContext
                {
                    Context = context,
                    Session = Session,
                    CurrentCulture = currentCulture,
                    Browser = this.Request.Browser
                };

            return dataContext;
        }

        public void PostMessage(MessageType messageType)
        {
            TempDataEntity model = new TempDataEntity();
            model.TipoMensaje = messageType;
            switch (messageType)
            {
                case MessageType.Success:
                    model.Mensaje = ConstantHelper.MENSAJE_EXITO;
                    break;
                case MessageType.Error:
                    model.Mensaje = ConstantHelper.MENSAJE_ERROR;
                    break;
            }

            var lastList = (List<TempDataEntity>)(TempData["Message"] ?? new List<TempDataEntity>());
            lastList.Add(model);

            TempData["Message"] = lastList;
        }

        public void PostMessage(MessageType messageType, String body = null)
        {
            TempDataEntity model = new TempDataEntity();
            model.TipoMensaje = messageType;
            model.Mensaje = body;

            var lastList = (List<TempDataEntity>)(TempData["Message"] ?? new List<TempDataEntity>());
            lastList.Add(model);

            TempData["Message"] = lastList;
        }

        public void PostMessage(CustomException exception)
        {
            CustomException model = new CustomException();

            var lastList = (List<CustomException>)(TempData["Exception"] ?? new List<CustomException>());
            lastList.Add(exception);
            TempData["Exception"] = lastList;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var context = GetDataContext();
            System.Exception ex = filterContext.Exception;
            var action = "Index";
            var controller = "Home";

            ExceptionHelper.LogException(ex, context);
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary
                                   {
                                       { "action", action },
                                       { "controller", controller },
                                       { "area", ""}
                                   });
            base.OnException(filterContext);
            PostMessage(MessageType.Error, ConstantHelper.MENSAJE_ERROR + " " + filterContext.Exception.Message);
            //PostMessage(MessageType.Warning, filterContext.Exception.Message);
        }

        public JsonResult AjaxException(TypeAjaxException type, Exception exception)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Response.StatusDescription = type.ToString();
            return Json(exception.Message);
        }
        public JsonResult AjaxException(TypeAjaxException type, String message)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Response.StatusDescription = type.ToString();
            return Json(message);
        }
        public enum TypeAjaxException
        {
            Error,
            Warning
        }
    }
}