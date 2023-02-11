using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCitas.Entidades; //referencia a la capa entidades
using GestionCitas.Datos; //referencia a la capa de datos
using Base.Comunes; //referencia a la capa comunes
namespace GestionCitas.Logica
{
    public class TipoServicioBLL //recuerden siempre crear la clase de forma publica
    {
        //patrón singleton
        #region singleton
        private static readonly TipoServicioBLL _instancia = new TipoServicioBLL();
        public static TipoServicioBLL Instancia
        {
            get { return TipoServicioBLL._instancia; }
        }
        #endregion singleton

        #region metodos
        public String Mensaje { get; private set; }//variable que nos permite guardar los mensajes de errores que vamos encontrando

        public Boolean Valida(TipoServicioDTO item)
        {
            Mensaje = "";
            Boolean resultado = false;

            if (String.IsNullOrWhiteSpace(item.Nombre))
                Mensaje += "Por favor, debe indicar un nombre\n\r";
            if (String.IsNullOrWhiteSpace(item.Descripcion))
                Mensaje += "Por favor, debe indicar la descripción\n\r";

            if (Mensaje == "") resultado = true;

            return resultado;
        }

        public List<TipoServicioDTO> ListarTipoServicios()
        {
            try
            {
                return TipoServicioDAL.Instancia.ListarTipoServicios();
            }
            catch (Exception e) { throw e; }
        }
        public RespuestaSistema GrabarTipoServicio(TipoServicioDTO item, String usuario)
        {
            RespuestaSistema objResultado = new RespuestaSistema();
            try
            {

                objResultado.Correcto = Valida(item);
                objResultado.Mensaje = Mensaje;
                if (objResultado.Correcto)
                {
                    int intPk;
                    if (item.Id == -1)//si es un registro nuevo, entonces registro la TipoServicio
                    {
                        intPk = TipoServicioDAL.Instancia.GrabarTipoServicio(item, usuario);
                        if (intPk > 0)//si se retorno un pk, entonces todo salio bien
                        {
                            objResultado.Mensaje = MensajeSistema.OK_SAVE;
                            objResultado.Correcto = true;
                        }
                        else// algo ocurrio y no se pudo registrar, devolver mensaje de error
                        {
                            objResultado.Mensaje = MensajeSistema.ERROR_SAVE;
                            objResultado.Correcto = false;
                        }
                    }
                    else //en caso contrario, es una TipoServicio ya creada por lo tanto la acción a realizar es la de modificar
                    {
                        objResultado.Correcto = TipoServicioDAL.Instancia.EditarTipoServicio(item, usuario);
                        if (objResultado.Correcto)//si se retorno true entonces, mensaje de éxito
                            objResultado.Mensaje = MensajeSistema.OK_UPDATE;
                        else// algo ocurrio y no se pudo modificar, devolver mensaje de error
                            objResultado.Mensaje = MensajeSistema.ERROR_UPDATE;
                    }
                }

            }
            catch (Exception ex)
            {
                objResultado.Mensaje = string.Format("{0}\r{1}", MensajeSistema.ERROR_SAVE, ex.Message);
                objResultado.Correcto = false;
            }
            return objResultado;//retorno la respuesta de registro o modificación
        }

        public TipoServicioDTO ObtenerTipoServicio(Int32 tipoServicioId)
        {
            try
            {
                return TipoServicioDAL.Instancia.ObtenerTipoServicio(tipoServicioId);
            }
            catch (Exception e) { throw e; }
        }
        #endregion
    }
}
