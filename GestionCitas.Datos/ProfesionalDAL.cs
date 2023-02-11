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
    public class ProfesionalDAL
    {
        #region singleton
        private static readonly ProfesionalDAL _instancia = new ProfesionalDAL();
        public static ProfesionalDAL Instancia
        {
            get { return ProfesionalDAL._instancia; }
        }
        #endregion singleton

        #region metodos

        public List<ProfesionalDTO> ListarProfesionales()
        {
            SqlCommand cmd = null;
            List<ProfesionalDTO> lista = new List<ProfesionalDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_PROFESIONALES_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ProfesionalDTO objProfesional = new ProfesionalDTO();
                    objProfesional.Id = Convert.ToInt16(dr["ID"]);
                    objProfesional.Nombres = dr["NOMBRES"].ToString();
                    objProfesional.Apellidos = dr["APELLIDOS"].ToString();
                    objProfesional.Dni = dr["DNI"].ToString();
                    objProfesional.Direccion = dr["DIRECCION"].ToString();
                    objProfesional.Correo = dr["CORREO"].ToString();
                    objProfesional.Telefono = dr["TELEFONO"].ToString();
                    objProfesional.Sexo = dr["SEXO"].ToString();
                    objProfesional.NumeroColegiatura = dr["NUMCOLEGIATURA"].ToString();
                    objProfesional.FechaNacimiento = Convert.ToDateTime(dr["FECHANACIMIENTO"].ToString());
                    objProfesional.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                    lista.Add(objProfesional);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return lista;
        }

        public Int32 GrabarProfesional(ProfesionalDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Int16 PKCreado = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_INSERTAR_PROFESIONAL_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOMBRES", item.Nombres);
                cmd.Parameters.AddWithValue("@APELLIDOS", item.Apellidos);
                cmd.Parameters.AddWithValue("@DNI", item.Dni);
                cmd.Parameters.AddWithValue("@DIRECCION", item.Direccion);
                cmd.Parameters.AddWithValue("@CORREO", item.Correo);
                cmd.Parameters.AddWithValue("@TELEFONO", item.Telefono);
                cmd.Parameters.AddWithValue("@SEXO", item.Sexo);
                cmd.Parameters.AddWithValue("@NUMCOLEGIATURA", item.NumeroColegiatura);
                cmd.Parameters.AddWithValue("@FECHANACIMIENTO", item.FechaNacimiento);
                cmd.Parameters.AddWithValue("@USUARIOREGISTRO", usuario);

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

        public Boolean EditarProfesional(ProfesionalDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Boolean modifico = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_EDITAR_PROFESIONAL_SP", cn);
                cmd.Parameters.AddWithValue("@ID", item.Id);
                cmd.Parameters.AddWithValue("@NOMBRES", item.Nombres);
                cmd.Parameters.AddWithValue("@APELLIDOS", item.Apellidos);
                cmd.Parameters.AddWithValue("@DNI", item.Dni);
                cmd.Parameters.AddWithValue("@DIRECCION", item.Direccion);
                cmd.Parameters.AddWithValue("@CORREO", item.Correo);
                cmd.Parameters.AddWithValue("@TELEFONO", item.Telefono);
                cmd.Parameters.AddWithValue("@SEXO", item.Sexo);
                cmd.Parameters.AddWithValue("@NUMCOLEGIATURA", item.NumeroColegiatura);
                cmd.Parameters.AddWithValue("@FECHANACIMIENTO", item.FechaNacimiento);
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

        public ProfesionalDTO ObtenerProfesional(Int32 profesionalId)
        {
            SqlCommand cmd = null;
            ProfesionalDTO item = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_PROFESIONAL_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", profesionalId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    item = new ProfesionalDTO();
                    item.Id = Convert.ToInt16(dr["ID"]);
                    item.Nombres = dr["NOMBRES"].ToString();
                    item.Apellidos = dr["APELLIDOS"].ToString();
                    item.Dni = dr["DNI"].ToString();
                    item.Direccion = dr["DIRECCION"].ToString();
                    item.Correo = dr["CORREO"].ToString();
                    item.Telefono = dr["TELEFONO"].ToString();
                    item.Sexo = dr["SEXO"].ToString();
                    item.NumeroColegiatura = dr["NUMCOLEGIATURA"].ToString();
                    item.FechaNacimiento = Convert.ToDateTime(dr["FECHANACIMIENTO"].ToString());
                    item.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                    item.ListaTipoServicios = ProfesionalTipoServicioDAL.Instancia.ListarTipoServiciosPorProfesional(profesionalId).Where(x => x.Activo == true).ToList();
                }
                return item;
            }
            catch (Exception e) { throw e; }
            finally { if (cmd != null) { cmd.Connection.Close(); } }
        }

        public List<ProfesionalDTO> ListarProfesionalesPorHorario(String opcionBusqueda, DateTime fechaAtencion, String filtro)
        {
            SqlCommand cmd = null;
            List<ProfesionalDTO> lista = new List<ProfesionalDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_BUSCAR_HORARIO", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TIPOBUSQUEDA", opcionBusqueda);
                cmd.Parameters.AddWithValue("@FECHAATENCION", fechaAtencion);
                cmd.Parameters.AddWithValue("@FILTRO", filtro);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ProfesionalDTO objProfesional = new ProfesionalDTO();
                    objProfesional.Id = Convert.ToInt16(dr["PROFESIONALID"]);
                    objProfesional.Nombres = dr["NOMBRES"].ToString();
                    objProfesional.Apellidos = dr["APELLIDOS"].ToString();
                    lista.Add(objProfesional);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return lista;
        }
        #endregion
    }
}
