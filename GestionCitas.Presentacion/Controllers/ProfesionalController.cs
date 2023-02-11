using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionCitas.Logica;
using GestionCitas.Entidades;

namespace GestionCitas.Presentacion.Controllers
{
    public class ProfesionalController : Controller
    {
        //
        // GET: /Profesional/
        public ActionResult Index()
        {
            return View();
        }

        #region CORE: Registrar Horarios
        public ActionResult VisualizarProfesionales()
        {
            return View();
        }

        public ActionResult Horario(String profesionalId)
        {
            if (profesionalId == null || profesionalId == "0")
            {
                return RedirectToAction("VisualizarProfesionales");
            }
            else
            {
                ViewBag.profesionalId = profesionalId;
                ProfesionalDTO objProfesional = ProfesionalBLL.Instancia.ObtenerProfesional(Convert.ToInt32(profesionalId));
                if (objProfesional != null)
                {
                    ViewBag.Head = "Horario del profesional: " + objProfesional.Apellidos + " " + objProfesional.Nombres;
                    ViewBag.Profesional = objProfesional.Apellidos + " " + objProfesional.Nombres;
                }
                else ViewBag.Head = "Solicitud no valida";
            }
            return View();
        }

        public ActionResult GrabarHorario(HorarioDTO item)
        {
            var obj = HorarioBLL.Instancia.GrabarHorario(item, "prueba");
            return Json(new { obj });
        }
        public ActionResult ObtenerHorario(Int32 horarioId)
        {
            var obj = HorarioBLL.Instancia.ObtenerHorario(horarioId);
            return Json(new { obj });
        }
        public ActionResult ListarHorarioProfesional(Int32 profesionalId)
        {
            var items = (from temp in HorarioBLL.Instancia.ListarHorariosPorProfesional(profesionalId)
                         select new
                         {
                             horarioId = temp.Id,
                             title = "Horario Registrado",
                             start = temp.FechaAtencion.ToString("yyy-MM-dd") + "T" + (temp.HoraInicio.Hours < 10 ? "0" + temp.HoraInicio.Hours.ToString() : temp.HoraInicio.Hours.ToString()) + ":" + (temp.HoraInicio.Minutes < 10 ? "0" + temp.HoraInicio.Minutes.ToString() : temp.HoraInicio.Minutes.ToString()),
                             end = temp.FechaAtencion.ToString("yyy-MM-dd") + "T" + (temp.HoraFin.Hours < 10 ? "0" + temp.HoraFin.Hours.ToString() : temp.HoraFin.Hours.ToString()) + ":" + (temp.HoraFin.Minutes < 10 ? "0" + temp.HoraFin.Minutes.ToString() : temp.HoraFin.Minutes.ToString()),
                             allDay = false,
                             backgroundColor = "#00c0ef",
                             borderColor = "#00c0ef",
                         });
            return Json(items.ToList());
        }

        #endregion CORE: Registrar Horarios

        public ActionResult ListarProfesionalesActivos()
        {
            var items = (from temp in ProfesionalBLL.Instancia.ListarProfesionales().Where(x => x.Activo == true)
                         select new
                         {
                             Id = temp.Id,
                             Nombres = temp.Nombres,
                             Apellidos = temp.Apellidos,
                             Dni = temp.Dni,
                             Telefono = temp.Telefono,
                             NumeroColegiatura = temp.NumeroColegiatura,
                             Activo = temp.Activo
                         });
            return Json(items.ToList());
        }
        public ActionResult ListarProfesionales()
        {
            var items = (from temp in ProfesionalBLL.Instancia.ListarProfesionales()
                         select new
                         {
                             Id = temp.Id,
                             Nombres = temp.Nombres,
                             Apellidos = temp.Apellidos,
                             Dni = temp.Dni,
                             Telefono = temp.Telefono,
                             NumeroColegiatura = temp.NumeroColegiatura,
                             Activo = temp.Activo
                         });
            return Json(items.ToList());
        }
        public ActionResult ObtenerProfesional(Int32 profesionalId)
        {
            var obj = ProfesionalBLL.Instancia.ObtenerProfesional(profesionalId);
            return Json(new { obj });
        }
        public ActionResult GrabarProfesional(ProfesionalDTO item)
        {
            var obj = ProfesionalBLL.Instancia.GrabarProfesional(item, "prueba");
            return Json(new { obj });
        }
        public ActionResult EliminarProfesional(ProfesionalDTO item)
        {
            item = ProfesionalBLL.Instancia.ObtenerProfesional(item.Id);
            var obj = ProfesionalBLL.Instancia.EliminarProfesional(item, "prueba");
            return Json(new { obj });
        }
        public ActionResult BuscarTipoServicio(String cadena)
        {
            cadena = cadena.ToLower();
            List<TipoServicioDTO> lista = TipoServicioBLL.Instancia.ListarTipoServicios().Where(x => x.Activo == true && x.Nombre.ToLower().Contains(cadena)).ToList();
            var items = (from temp in lista
                         select new
                         {
                             Id = temp.Id,
                             Nombre = temp.Nombre,
                             Activo = temp.Activo
                         });
            return Json(items.ToList());
        }
	}
}