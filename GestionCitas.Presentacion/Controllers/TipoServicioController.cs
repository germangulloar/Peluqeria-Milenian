using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionCitas.Logica; //importamos la capa lógica
using GestionCitas.Entidades; //importamos la capa entidades

namespace GestionCitas.Presentacion.Controllers
{
    public class TipoServicioController : Controller
    {
        //
        // GET: /TipoServicio/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarTipoServicios()
        {
            var items = (from temp in TipoServicioBLL.Instancia.ListarTipoServicios()
                         select new
                         {
                             Id = temp.Id,
                             Nombre = temp.Nombre,
                             Descripcion = temp.Descripcion,
                             Activo = temp.Activo
                         });
            return Json(items.ToList());
        }

        public ActionResult ObtenerTipoServicio(Int16 tipoServicioId)
        {
            var obj = TipoServicioBLL.Instancia.ObtenerTipoServicio(tipoServicioId);
            return Json(new { obj });
        }
        public ActionResult GrabarTipoServicio(TipoServicioDTO item)
        {
            var obj = TipoServicioBLL.Instancia.GrabarTipoServicio(item, "prueba");
            return Json(new { obj });
        }
	}
}