namespace Cursos.Models
{
    using Cursos.BDConnection;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public partial class usuarios
    {
        public int id { get; set; }

        [StringLength(20)]
        public string rol { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(50)]
        public string apellido { get; set; }

        [StringLength(255)]
        public string email { get; set; }

        [StringLength(20)]
        public string pass { get; set; }

        [StringLength(20)]
        public string passConfirm { get; set; }

        [StringLength(20)]
        public string matricula { get; set; }

        [StringLength(50)]
        public string carrera { get; set; }

        [StringLength(20)]
        public string estudios { get; set; }

        //public virtual rol rol1 { get; set; }

       

        //Metodos
        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;

        //Metodo para obtener datos del usuario
        public List<usuarios> GetUsuarios()
        {
            List<usuarios> usuarios = new List<usuarios>();

            string sql = "select * from usuarios";
            using(SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using(SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usuarios u = new usuarios();
                        rol r = new rol();
                        u.id = dr.GetInt32(0);
                        u.rol = dr.GetString(1);
                        u.nombre = dr.GetString(2);
                        u.apellido = dr.GetString(3);
                        u.email = dr.GetString(4);
                        u.pass = dr.GetString(5);
                        u.matricula= dr.GetString(6);
                        u.carrera= dr.GetString(7);
                        u.estudios= dr.GetString(8);
                        usuarios.Add(u);
                    }
                }
            }
            return usuarios;
        }

        public List<usuarios> GetUsuario()
        {
            List<usuarios> usu = new List<usuarios>();
            usuarios usuario = (usuarios)HttpContext.Current.Session["usuario"];
            string sql = "select * from usuarios where usuarios.id = " + usuario.id + ";";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usuarios u = new usuarios();
                        u.id = dr.GetInt32(0);
                        u.rol = dr.GetString(1);
                        u.nombre = dr.GetString(2);
                        u.apellido = dr.GetString(3);
                        u.email = dr.GetString(4);
                        u.pass = dr.GetString(5);
                        u.matricula = dr.GetString(6);
                        u.carrera = dr.GetString(7);
                        u.estudios = dr.GetString(8);
                        usu.Add(u);
                    }
                }
            }
            return usu;
        }

        //Metodo para agregar usuarios
        public bool InsertaUsuario(usuarios usuarioInfo)
        {
            bool registrado;
            string mensaje;
            int i;
            if (usuarioInfo.pass == usuarioInfo.passConfirm)
            {
                usuarioInfo.pass = EncriptarSha256(usuarioInfo.pass);
            }
            else
            {
                return false;
            }

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_registraUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Rol", usuarioInfo.rol);
                cmd.Parameters.AddWithValue("Nombre", usuarioInfo.nombre);
                cmd.Parameters.AddWithValue("Apellido", usuarioInfo.apellido);
                cmd.Parameters.AddWithValue("Email", usuarioInfo.email);
                cmd.Parameters.AddWithValue("Pass", usuarioInfo.pass);
                cmd.Parameters.AddWithValue("Matricula", usuarioInfo.matricula);
                cmd.Parameters.AddWithValue("Carrera", usuarioInfo.carrera);
                cmd.Parameters.AddWithValue("Estudios", usuarioInfo.estudios);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                

                cn.Open();
                i = cmd.ExecuteNonQuery();
                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                cn.Close();
            }

            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Metodo para encriptar password
        public static string EncriptarSha256(String texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        //Metodo para editar usuario
        public bool EditarUsuario(usuarios usuarioInfo) 
        {

            bool editado;
            string mensaje;
            int i;
            if (usuarioInfo.pass == usuarioInfo.passConfirm)
            {
                usuarioInfo.pass = EncriptarSha256(usuarioInfo.pass);

            }
            else
            {
                return false;
            }

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_editaUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", usuarioInfo.id);
                cmd.Parameters.AddWithValue("Rol", usuarioInfo.rol);
                cmd.Parameters.AddWithValue("Nombre", usuarioInfo.nombre);
                cmd.Parameters.AddWithValue("Apellido", usuarioInfo.apellido);
                cmd.Parameters.AddWithValue("Email", usuarioInfo.email);
                cmd.Parameters.AddWithValue("Pass", usuarioInfo.pass);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
              
                cn.Open();
                i = cmd.ExecuteNonQuery();
                editado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                cn.Close();
            }

            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Metodo para eliminar usuario
        public bool eliminarUsuario(int id)
        {
            int i;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_eliminaUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", id);


                cn.Open();
                i = cmd.ExecuteNonQuery();
                cn.Close();
            }

            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }   
}
