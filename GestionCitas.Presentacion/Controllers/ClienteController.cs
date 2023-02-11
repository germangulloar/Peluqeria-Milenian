using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionCitas.Logica;
using GestionCitas.Entidades;

namespace GestionCitas.Presentacion.Controllers
{
    public class ClienteController : Controller
    {
        //
        // GET: /Cliente/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListarClientes()
        {
            var items = (from temp in ClienteBLL.Instancia.ListarClientes().Where(x => x.Activo == true).ToList()
                         select new
                         {
                             Id = temp.Id,
                             Nombres = temp.Nombres,
                             Apellidos = temp.Apellidos,
                             Dni = temp.Dni,
                             Telefono = temp.Telefono,
                             Activo = (temp.Activo ? "Activo" : "Inactivo")
                         });
            return Json(items.ToList());
        }
        public ActionResult ObtenerCliente(Int16 profesionalId)
        {
            var obj = ClienteBLL.Instancia.ObtenerCliente(profesionalId);
            return Json(new { obj });
        }
        public ActionResult GrabarCliente(ClienteDTO item)
        {
            var obj = ClienteBLL.Instancia.GrabarCliente(item, "prueba");
            return Json(new { obj });
        }
        public ActionResult EliminarCliente(ClienteDTO item)
        {
            item = ClienteBLL.Instancia.ObtenerCliente(item.Id);
            var obj = ClienteBLL.Instancia.EliminarCliente(item, "prueba");
            return Json(new { obj });
        }
	}
}