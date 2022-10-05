namespace Cursos.Models
{
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

    public partial class usuarios
    {
        public int id { get; set; }

        public int rol { get; set; }

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

        public virtual rol rol1 { get; set; }


        //Metodos
        static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq;Integrated Security=true;";
        //static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Database=edcouteq; user id=adminedco; pwd=edco_uteq_2022**";

        //Metodo para obtener datos del usuario
        public List<usuarios> GetUsuarios()
        {
            List<usuarios> usuarios = new List<usuarios>();
            List<rol> rol = new List<rol>();

            string sql = "select usuarios.id, usuarios.nombre, usuarios.apellido, rol.id, rol.rolUser, usuarios.email, usuarios.pass from usuarios inner join rol on rol.id = usuarios.rol;";
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
                        u.nombre = dr.GetString(1);
                        u.apellido = dr.GetString(2);
                        r.id = dr.GetInt32(3);
                        r.rolUser = dr.GetString(4);
                        u.rol1 = r;
                        u.email = dr.GetString(5);
                        u.pass = dr.GetString(6);
                        usuarios.Add(u);
                    }
                }
            }
            return usuarios;
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
