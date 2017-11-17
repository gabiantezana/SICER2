using System.Web.Mvc;

namespace SICER.Areas.GestionDocumentos
{
    public class GestionDocumentosAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "GestionDocumentos";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GestionDocumentos_default",
                "GestionDocumentos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}