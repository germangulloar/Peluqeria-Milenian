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
    public class ClienteDAL
    {
        #region singleton
        private static readonly ClienteDAL _instancia = new ClienteDAL();
        public static ClienteDAL Instancia
        {
            get { return ClienteDAL._instancia; }
        }
        #endregion singleton

        #region metodos
        public List<ClienteDTO> ListarClientes()
        {
            SqlCommand cmd = null;
            List<ClienteDTO> lista = new List<ClienteDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_CLIENTES_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ClienteDTO objCliente = new ClienteDTO();
                    objCliente.Id = Convert.ToInt16(dr["ID"]);
                    objCliente.Nombres = dr["NOMBRES"].ToString();
                    objCliente.Apellidos = dr["APELLIDOS"].ToString();
                    objCliente.Dni = dr["DNI"].ToString();
                    objCliente.Direccion = dr["DIRECCION"].ToString();
                    objCliente.Telefono = dr["TELEFONO"].ToString();
                    objCliente.Sexo = dr["SEXO"].ToString();
                    objCliente.FechaNacimiento = Convert.ToDateTime(dr["FECHANACIMIENTO"].ToString());
                    objCliente.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                    lista.Add(objCliente);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return lista;
        }

        public Int32 GrabarCliente(ClienteDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Int16 PKCreado = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_INSERTAR_CLIENTE_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOMBRES", item.Nombres);
                cmd.Parameters.AddWithValue("@APELLIDOS", item.Apellidos);
                cmd.Parameters.AddWithValue("@DNI", item.Dni);
                cmd.Parameters.AddWithValue("@DIRECCION", item.Direccion);
                cmd.Parameters.AddWithValue("@TELEFONO", item.Telefono);
                cmd.Parameters.AddWithValue("@SEXO", item.Sexo);
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

        public Boolean EditarCliente(ClienteDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Boolean modifico = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_EDITAR_CLIENTE_SP", cn);
                cmd.Parameters.AddWithValue("@ID", item.Id);
                cmd.Parameters.AddWithValue("@NOMBRES", item.Nombres);
                cmd.Parameters.AddWithValue("@APELLIDOS", item.Apellidos);
                cmd.Parameters.AddWithValue("@DNI", item.Dni);
                cmd.Parameters.AddWithValue("@DIRECCION", item.Direccion);
                cmd.Parameters.AddWithValue("@TELEFONO", item.Telefono);
                cmd.Parameters.AddWithValue("@SEXO", item.Sexo);
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

        public ClienteDTO ObtenerCliente(Int32 profesionalId)
        {
            SqlCommand cmd = null;
            ClienteDTO item = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_CLIENTE_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", profesionalId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    item = new ClienteDTO();
                    item.Id = Convert.ToInt16(dr["ID"]);
                    item.Nombres = dr["NOMBRES"].ToString();
                    item.Apellidos = dr["APELLIDOS"].ToString();
                    item.Dni = dr["DNI"].ToString();
                    item.Direccion = dr["DIRECCION"].ToString();
                    item.Telefono = dr["TELEFONO"].ToString();
                    item.Sexo = dr["SEXO"].ToString();
                    item.FechaNacimiento = Convert.ToDateTime(dr["FECHANACIMIENTO"].ToString());
                    item.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                }
                return item;
            }
            catch (Exception e) { throw e; }
            finally { if (cmd != null) { cmd.Connection.Close(); } }
        }
        #endregion
    }
}
