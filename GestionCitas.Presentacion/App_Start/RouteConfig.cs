using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GestionCitas.Presentacion
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(null, "{controller}/{action}");

            routes.MapRoute(
                "Profesional",
                "Profesional/{action}/{profesionalId}",
                new { controller = "Profesional", action = "Horario", profesionalId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Cita",
                "Cita/{action}/{profesionalId}/{fechaAtencion}",
                new { controller = "Cita", action = "Horario", profesionalId = UrlParameter.Optional, fechaAtencion = UrlParameter.Optional }
            );
        }
    }
}
