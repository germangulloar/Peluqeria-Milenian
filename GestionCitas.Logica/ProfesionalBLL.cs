using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCitas.Entidades;
using GestionCitas.Datos;
using Base.Comunes;
using System.Transactions;

namespace GestionCitas.Logica
{
    public class ProfesionalBLL
    {
        #region singleton
        private static readonly ProfesionalBLL _instancia = new ProfesionalBLL();
        public static ProfesionalBLL Instancia
        {
            get { return ProfesionalBLL._instancia; }
        }
        #endregion singleton

        #region metodos

        public String Mensaje { get; private set; }

        public Boolean Valida(ProfesionalDTO item)
        {
            Mensaje = "";
            Boolean resultado = false;
            int numRegistrosAEliminar = 0;

            if (String.IsNullOrWhiteSpace(item.Dni))
                Mensaje += "Por favor, debe indicar el DNI\n\r";
            if (String.IsNullOrWhiteSpace(item.NumeroColegiatura))
                Mensaje += "Por favor, debe indicar el N° de colegiatura\n\r";
            if (String.IsNullOrWhiteSpace(item.Nombres))
                Mensaje += "Por favor, debe indicar un nombre\n\r";
            if (String.IsNullOrWhiteSpace(item.Apellidos))
                Mensaje += "Por favor, debe indicar los apellidos\n\r";
            if (item.FechaNacimiento == DateTime.Parse("0001-01-01"))
                Mensaje += "Por favor, debe indicar la fecha de nacimiento\n\r";
            if (String.IsNullOrWhiteSpace(item.Sexo))
                Mensaje += "Por favor, debe indicar el sexo\n\r";
            if (String.IsNullOrWhiteSpace(item.Telefono))
                Mensaje += "Por favor, debe indicar el teléfono\n\r";
            if (String.IsNullOrWhiteSpace(item.Direccion))
                Mensaje += "Por favor, debe indicar la dirección\n\r";
            if (String.IsNullOrWhiteSpace(item.Telefono))
                Mensaje += "Por favor, debe indicar el teléfono\n\r";

            if (!String.IsNullOrWhiteSpace(item.Dni))
            {
                List<ProfesionalDTO> ListadoProfesionales = ProfesionalBLL.Instancia.ListarProfesionales().Where(x => x.Dni == item.Dni && x.Id != item.Id).ToList();
                if (ListadoProfesionales.Count > 0)
                    Mensaje += "El DNI Ingresado ya se encuentra asignado a otro médico\n\r";
            }

            if (item.ListaTipoServicios != null && item.ListaTipoServicios.Count > 0)
                numRegistrosAEliminar = item.ListaTipoServicios.Where(x => x.Activo == false).ToList().Count;

            if (item.ListaTipoServicios != null && item.ListaTipoServicios.Count > 0 && (numRegistrosAEliminar != item.ListaTipoServicios.Count))
            {
                int numTipoServicios = item.ListaTipoServicios.Count;
                Boolean esRegistroDuplicado = false;
                for (int i = 0; i < numTipoServicios; i++)
                {
                    if (!item.ListaTipoServicios[i].Activo) continue;

                    if (item.ListaTipoServicios[i].TipoServicioId == 0)
                    {
                        Mensaje += "Por favor, debe indicar el nombre del servicio\n\r";
                        break;
                    }
                    if (String.IsNullOrWhiteSpace(item.ListaTipoServicios[i].TipoServicio))
                    {
                        Mensaje += "Por favor, debe indicar el nombre del servicio\n\r";
                        break;
                    }

                    for (int j = 0; j < numTipoServicios; j++)
                    {
                        if (!item.ListaTipoServicios[j].Activo) continue;
                        if (i != j)
                        {
                            if (item.ListaTipoServicios[i].TipoServicioId == item.ListaTipoServicios[j].TipoServicioId)
                            {
                                Mensaje += "El Servicio '" + item.ListaTipoServicios[i].TipoServicio + "'\n\rse repite en la lista de servicios\n\r";
                                esRegistroDuplicado = true;
                                break;
                            }
                        }
                    }
                    if (esRegistroDuplicado) break;
                }
            }
            else
                Mensaje += "Por favor, debe indicar al menos un servicio\n\r";

            if (Mensaje == "") resultado = true;

            return resultado;
        }

        public List<ProfesionalDTO> ListarProfesionales()
        {
            try
            {
                return ProfesionalDAL.Instancia.ListarProfesionales();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public RespuestaSistema GrabarProfesional(ProfesionalDTO item, String usuario)
        {
            RespuestaSistema objResultado = new RespuestaSistema();
            Int32 pkInsertado = -1;
            Boolean resultado = false;
            Mensaje = "";
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    resultado = Valida(item);
                    objResultado.Mensaje = Mensaje;
                    if (resultado)
                    {
                        if (item.Id == -1)
                        {
                            pkInsertado = ProfesionalDAL.Instancia.GrabarProfesional(item, usuario);
                            if (pkInsertado > 0)
                            {
                                foreach (ProfesionalTipoServicioDTO objTipoServicio in item.ListaTipoServicios)
                                {
                                    resultado = ProfesionalTipoServicioDAL.Instancia.GrabarTipoServicioDeProfesional(objTipoServicio, pkInsertado, usuario);
                                    if (resultado == false)
                                    {
                                        objResultado.Mensaje = string.Format("{0}\r", MensajeSistema.ERROR_UPDATE);
                                        objResultado.Correcto = resultado;
                                        return objResultado;
                                    }
                                }
                                if (resultado)
                                {
                                    objResultado.Mensaje = MensajeSistema.OK_SAVE;
                                    transactionScope.Complete();
                                }
                                else
                                {
                                    objResultado.Mensaje = MensajeSistema.ERROR_SAVE;
                                }
                            }
                            else
                            {
                                objResultado.Correcto = false;
                                objResultado.Mensaje = MensajeSistema.ERROR_UPDATE;
                            }
                        }
                        else
                        {
                            resultado = ProfesionalDAL.Instancia.EditarProfesional(item, usuario);
                            if (resultado)
                            {
                                foreach (ProfesionalTipoServicioDTO objTipoServicio in item.ListaTipoServicios)
                                {
                                    if (objTipoServicio.Id == 0)
                                        resultado = ProfesionalTipoServicioDAL.Instancia.GrabarTipoServicioDeProfesional(objTipoServicio, item.Id, usuario);
                                    else
                                        resultado = ProfesionalTipoServicioDAL.Instancia.EditarTipoServicio(objTipoServicio, item.Id, usuario);
                                    if (resultado == false)
                                    {
                                        objResultado.Mensaje = string.Format("{0}\r", MensajeSistema.ERROR_UPDATE);
                                        objResultado.Correcto = resultado;
                                        return objResultado;
                                    }
                                }
                                if (resultado)
                                {
                                    objResultado.Mensaje = MensajeSistema.OK_UPDATE;
                                    transactionScope.Complete();
                                }
                                else Mensaje = MensajeSistema.ERROR_UPDATE;
                            }
                            else
                            {
                                objResultado.Correcto = false;
                                objResultado.Mensaje = MensajeSistema.ERROR_UPDATE;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    objResultado.Mensaje = string.Format("{0}\r{1}", MensajeSistema.ERROR_SAVE, ex.Message);
                    resultado = false;
                }
                objResultado.Correcto = resultado;

                return objResultado;
            }
        }

        public RespuestaSistema EliminarProfesional(ProfesionalDTO item, String usuario)
        {
            RespuestaSistema objResultado = new RespuestaSistema();
            Boolean resultado = false;
            Mensaje = "";
            item.Activo = false;
            try
            {
                resultado = ProfesionalDAL.Instancia.EditarProfesional(item, usuario);
                if (resultado)
                    Mensaje += MensajeSistema.OK_DELETE;
                else Mensaje += MensajeSistema.ERROR_DELETE;
            }
            catch (Exception e)
            {
                throw e;
            }
            objResultado.Mensaje = Mensaje;
            objResultado.Correcto = resultado;
            return objResultado;
        }
        public ProfesionalDTO ObtenerProfesional(Int32 profesionalId)
        {
            try
            {
                return ProfesionalDAL.Instancia.ObtenerProfesional(profesionalId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ProfesionalDTO> ListarProfesionalesPorHorario(String opcionBusqueda, DateTime fechaAtencion, String filtro)
        {
            try
            {
                return ProfesionalDAL.Instancia.ListarProfesionalesPorHorario(opcionBusqueda, fechaAtencion, filtro);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion
    }
}
