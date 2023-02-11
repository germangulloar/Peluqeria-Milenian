using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using GestionCitas.Entidades;

namespace GestionCitas.Datos
{
    public class TipoServicioDAL
    {
        #region patron singleton
        private static readonly TipoServicioDAL _instancia = new TipoServicioDAL();
        public static TipoServicioDAL Instancia
        {
            get { return TipoServicioDAL._instancia; }
        }
        #endregion

        #region metodos
        public List<TipoServicioDTO> ListarTipoServicios()
        {
            SqlCommand cmd = null;
            List<TipoServicioDTO> lista = new List<TipoServicioDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_TIPOSERVICIOS_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    TipoServicioDTO objTipoServicio = new TipoServicioDTO();
                    objTipoServicio.Id = Convert.ToInt16(dr["ID"]);
                    objTipoServicio.Nombre = dr["NOMBRE"].ToString();
                    objTipoServicio.Descripcion = dr["DESCRIPCION"].ToString();
                    objTipoServicio.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                    lista.Add(objTipoServicio);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return lista;
        }
        public TipoServicioDTO ObtenerTipoServicio(Int32 TipoServicioId)
        {
            SqlCommand cmd = null;
            TipoServicioDTO item = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_TIPOSERVICIO_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", TipoServicioId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    item = new TipoServicioDTO();
                    item.Id = Convert.ToInt16(dr["ID"]);
                    item.Nombre = dr["NOMBRE"].ToString();
                    item.Descripcion = dr["DESCRIPCION"].ToString();
                    item.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                }
                return item;
            }
            catch (Exception e) { throw e; }
            finally { if (cmd != null) { cmd.Connection.Close(); } }
        }

        public Int32 GrabarTipoServicio(TipoServicioDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Int16 PKCreado = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_INSERTAR_TIPOSERVICIO_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOMBRE", item.Nombre);
                cmd.Parameters.AddWithValue("@DESCRIPCION", item.Descripcion);

                cn.Open();
                PKCreado = Convert.ToInt16(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return PKCreado;
        }

        public Boolean EditarTipoServicio(TipoServicioDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Boolean modifico = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_EDITAR_TIPOSERVICIO_SP", cn);
                cmd.Parameters.AddWithValue("@ID", item.Id);
                cmd.Parameters.AddWithValue("@NOMBRE", item.Nombre);
                cmd.Parameters.AddWithValue("@DESCRIPCION", item.Descripcion);
                cmd.Parameters.AddWithValue("@ACTIVO", item.Activo);
                cmd.Parameters.AddWithValue("@USUARIOMODIFICACION", usuario);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0) { modifico = true; }
                return modifico;
            }
            catch (Exception e) { throw e; }
            finally { if (cmd != null) { cmd.Connection.Close(); } }
        }
        #endregion
    }
}
