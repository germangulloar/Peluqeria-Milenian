using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionCitas.Logica;
using GestionCitas.Entidades;
using System.Globalization;

namespace GestionCitas.Presentacion.Controllers
{
    public class CitaController : Controller
    {
        //
        // GET: /Cita/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ObtenerCita(Int32 citaId)
        {
            var obj = CitaBLL.Instancia.ObtenerCita(citaId);
            return Json(new { obj });
        }

        public ActionResult GrabarCita(CitaDTO item)
        {
            var obj = CitaBLL.Instancia.GrabarCita(item, "prueba");
            return Json(new { obj });
        }

        public ActionResult Horario(String profesionalId, String fechaAtencion)
        {
            if (profesionalId == null || fechaAtencion == null || profesionalId == "0" || fechaAtencion == "")
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.profesionalId = profesionalId;
                ViewBag.fechaAtencion = fechaAtencion;
                ProfesionalDTO objprofesional = ProfesionalBLL.Instancia.ObtenerProfesional(Convert.ToInt32(profesionalId));
                ViewBag.Head = "Citas del médico: " + objprofesional.Apellidos + " " + objprofesional.Nombres;
                ViewBag.Profesional = objprofesional.Apellidos + " " + objprofesional.Nombres;
            }
            return View();
        }

        public ActionResult ListarCitasProfesional(Int32 profesionalId, String fechaAtencion)
        {
            DateTime fecha = DateTime.Parse(fechaAtencion);
            var horarioProfesional = (from temp in HorarioBLL.Instancia.ListarHorariosPorProfesional(profesionalId)
                                 select new
                                 {
                                     citaId = -1,
                                     title = "Horario Profesional",
                                     start = temp.FechaAtencion.ToString("yyy-MM-dd") + "T" + (temp.HoraInicio.Hours < 10 ? "0" + temp.HoraInicio.Hours.ToString() : temp.HoraInicio.Hours.ToString()) + ":" + (temp.HoraInicio.Minutes < 10 ? "0" + temp.HoraInicio.Minutes.ToString() : temp.HoraInicio.Minutes.ToString()),
                                     end = temp.FechaAtencion.ToString("yyy-MM-dd") + "T" + (temp.HoraFin.Hours < 10 ? "0" + temp.HoraFin.Hours.ToString() : temp.HoraFin.Hours.ToString()) + ":" + (temp.HoraFin.Minutes < 10 ? "0" + temp.HoraFin.Minutes.ToString() : temp.HoraFin.Minutes.ToString()),
                                     allDay = false,
                                     backgroundColor = "",
                                     borderColor = "",
                                     rendering = "background",
                                     color = "#ff9f89"
                                 });
            var items = (from temp in CitaBLL.Instancia.ListarCitasProfesional(profesionalId, fecha).Where(x => x.Estado == "PENDIENTE")
                         select new
                         {
                             citaId = temp.Id,
                             title = temp.ClienteApellidos + ' ' + temp.ClienteNombres,
                             start = temp.FechaAtencion.ToString("yyy-MM-dd") + "T" + (temp.HoraInicio.Hours < 10 ? "0" + temp.HoraInicio.Hours.ToString() : temp.HoraInicio.Hours.ToString()) + ":" + (temp.HoraInicio.Minutes < 10 ? "0" + temp.HoraInicio.Minutes.ToString() : temp.HoraInicio.Minutes.ToString()),
                             end = temp.FechaAtencion.ToString("yyy-MM-dd") + "T" + (temp.HoraFin.Hours < 10 ? "0" + temp.HoraFin.Hours.ToString() : temp.HoraFin.Hours.ToString()) + ":" + (temp.HoraFin.Minutes < 10 ? "0" + temp.HoraFin.Minutes.ToString() : temp.HoraFin.Minutes.ToString()),
                             allDay = false,
                             backgroundColor = "#00c0ef",
                             borderColor = "#00c0ef",
                             rendering = "",
                             color = ""
                         });
            return Json(items.Concat(horarioProfesional).ToList());
        }

        public ActionResult ListarProfesionalesPorHorario(String opcionBusqueda, DateTime fechaAtencion, String filtro)
        {
            var items = (from temp in ProfesionalBLL.Instancia.ListarProfesionalesPorHorario(opcionBusqueda, fechaAtencion, filtro)
                         select new
                         {
                             Id = temp.Id,
                             Nombres = temp.Nombres,
                             Apellidos = temp.Apellidos,
                         });
            return Json(items.ToList());
        }

        public ActionResult BuscarCliente(String cadena)
        {
            cadena = cadena.ToLower();
            List<ClienteDTO> lista = ClienteBLL
                                        .Instancia
                                        .ListarClientes()
                                        .Where(x => x.Activo == true && x.Dni.ToLower().Contains(cadena)).ToList();
            var items = (from temp in lista
                         select new
                         {
                             Id = temp.Id,
                             Nombre = temp.Apellidos + " " + temp.Nombres,
                             Dni = temp.Dni,
                             Activo = temp.Activo
                         });
            return Json(items.ToList());
        }
    }
}
