using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCitas.Entidades;
using GestionCitas.Datos;
using Base.Comunes;

namespace GestionCitas.Logica
{
    public class ClienteBLL
    {
        #region singleton
        private static readonly ClienteBLL _instancia = new ClienteBLL();
        public static ClienteBLL Instancia
        {
            get { return ClienteBLL._instancia; }
        }
        #endregion singleton

        #region metodos
        public String Mensaje { get; private set; }

        public Boolean Valida(ClienteDTO item)
        {
            Mensaje = "";
            Boolean resultado = false;

            if (String.IsNullOrWhiteSpace(item.Dni))
                Mensaje += "Por favor, debe indicar el DNI\n\r";
            if (String.IsNullOrWhiteSpace(item.Nombres))
                Mensaje += "Por favor, debe indicar un nombre\n\r";
            if (String.IsNullOrWhiteSpace(item.Apellidos))
                Mensaje += "Por favor, debe indicar los apellidos\n\r";
            if (item.FechaNacimiento == DateTime.Parse("0001-01-01"))
                Mensaje += "Por favor, debe indicar la fecha de nacimiento\n\r";
            if (String.IsNullOrWhiteSpace(item.Sexo))
                Mensaje += "Por favor, debe indicar el sexo\n\r";
            if (String.IsNullOrWhiteSpace(item.Direccion))
                Mensaje += "Por favor, debe indicar la dirección\n\r";
            if (String.IsNullOrWhiteSpace(item.Telefono))
                Mensaje += "Por favor, debe indicar el teléfono\n\r";

            if (!String.IsNullOrWhiteSpace(item.Dni))
            {
                List<ClienteDTO> ListadoClientes = ListarClientes().Where(x => x.Dni == item.Dni && x.Id != item.Id).ToList();
                if (ListadoClientes.Count > 0)
                    Mensaje += "El DNI Ingresado ya se encuentra asignado a otro cliente\n\r";
            }



            if (Mensaje == "") resultado = true;

            return resultado;
        }
        public List<ClienteDTO> ListarClientes()
        {
            try
            {
                return ClienteDAL.Instancia.ListarClientes();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public RespuestaSistema GrabarCliente(ClienteDTO item, String usuario)
        {
            RespuestaSistema objResultado = new RespuestaSistema();
            objResultado.Correcto = false;
            try
            {
                Mensaje = "";
                int intPk;
                objResultado.Correcto = Valida(item);
                objResultado.Mensaje = Mensaje;
                if (objResultado.Correcto)
                {
                    if (item.Id == -1)
                    {
                        intPk = ClienteDAL.Instancia.GrabarCliente(item, usuario);
                        if (intPk > 0)
                        {
                            objResultado.Mensaje = MensajeSistema.OK_SAVE;
                            objResultado.Correcto = true;
                        }
                    }
                    else
                    {
                        objResultado.Correcto = ClienteDAL.Instancia.EditarCliente(item, usuario);
                        if (objResultado.Correcto)
                            objResultado.Mensaje = objResultado.Mensaje = MensajeSistema.OK_UPDATE;
                        else
                            objResultado.Mensaje = objResultado.Mensaje = MensajeSistema.ERROR_UPDATE;
                    }
                }
            }
            catch (Exception ex)
            {
                objResultado.Mensaje = string.Format("{0}\r{1}", MensajeSistema.ERROR_SAVE, ex.Message);
                objResultado.Correcto = false;
            }
            return objResultado;
        }
        public ClienteDTO ObtenerCliente(Int32 tipoServicioId)
        {
            try
            {
                return ClienteDAL.Instancia.ObtenerCliente(tipoServicioId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public RespuestaSistema EliminarCliente(ClienteDTO item, String usuario)
        {
            RespuestaSistema objResultado = new RespuestaSistema();
            Boolean resultado = false;
            Mensaje = "";
            item.Activo = false;
            try
            {
                resultado = ClienteDAL.Instancia.EditarCliente(item, usuario);
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
        #endregion
    }
}
