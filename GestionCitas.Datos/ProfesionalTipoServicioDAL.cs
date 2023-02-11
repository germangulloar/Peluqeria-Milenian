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
    public class ProfesionalTipoServicioDAL
    {
        #region singleton
        private static readonly ProfesionalTipoServicioDAL _instancia = new ProfesionalTipoServicioDAL();
        public static ProfesionalTipoServicioDAL Instancia
        {
            get { return ProfesionalTipoServicioDAL._instancia; }
        }
        #endregion singleton

        #region metodos
        public List<ProfesionalTipoServicioDTO> ListarTipoServiciosPorProfesional(Int32 profesionalId)
        {
            SqlCommand cmd = null;
            List<ProfesionalTipoServicioDTO> lista = new List<ProfesionalTipoServicioDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_PROFESIONALES_TIPOSERVICIOS_POR_PROFESIONAL_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PROFESIONALID", profesionalId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ProfesionalTipoServicioDTO objTipoServicio = new ProfesionalTipoServicioDTO();
                    objTipoServicio.Id = Convert.ToInt16(dr["ID"]);
                    objTipoServicio.TipoServicioId = Convert.ToInt16(dr["TIPOSERVICIOID"]);
                    objTipoServicio.TipoServicio = dr["NOMBRE"].ToString();
                    objTipoServicio.TipoServicioDescripcion = dr["DESCRIPCION"].ToString();
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

        public Boolean GrabarTipoServicioDeProfesional(ProfesionalTipoServicioDTO item, Int32 profesionalId, String usuario)
        {
            SqlCommand cmd = null;
            Int16 PKCreado = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_INSERTAR_PROFESIONALES_TIPOSERVICIOS_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PROFESIONALID", profesionalId);
                cmd.Parameters.AddWithValue("@TIPOSERVICIOID", item.TipoServicioId);
                cmd.Parameters.AddWithValue("@USUARIOREGISTRO", usuario);
                cn.Open();
                PKCreado = Convert.ToInt16(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return (PKCreado > 0);
        }

        public Boolean EditarTipoServicio(ProfesionalTipoServicioDTO item, Int32 profesionalId, String usuario)
        {
            SqlCommand cmd = null;
            Boolean modifico = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_EDITAR_PROFESIONALES_TIPOSERVICIOS_SP", cn);
                cmd.Parameters.AddWithValue("@ID", item.Id);
                cmd.Parameters.AddWithValue("@PROFESIONALID", profesionalId);
                cmd.Parameters.AddWithValue("@TIPOSERVICIOID", item.TipoServicioId);
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
