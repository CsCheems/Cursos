using Cursos.BDConnection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cursos.Models
{
    public class ViewModelUsuario
    {
        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;
        public IEnumerable<usuario> usuario { get; set; }
        //public IEnumerable<rol> roles { get; set; }
        public List<usuario> GetCurrentUsuario()
        {
            List<usuario> usuarios = new List<usuario>();
            usuario usuario = (usuario)HttpContext.Current.Session["usuario"];
            string sql = "select * from usuarios where usuarios.id = " +usuario.id +";";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usuario u = new usuario();
                        
                        
                        usuarios.Add(u);
                    }
                }
            }
            return usuarios;
        }

        public List<usuario> GetUsuarios()
        {
            List<usuario> usuarios = new List<usuario>();
            

            string sql = "select * from usuario;";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usuario u = new usuario();
                        
                        usuarios.Add(u);
                    }
                }
            }
            return usuarios;
        }

        /*public List<rol> GetRoles()
        {
           
            List<rol> rol = new List<rol>();

            string sql = "select * from rol;";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        
                        rol r = new rol();
                        r.id = dr.GetInt32(0);
                        r.rolUser = dr.GetString(1);
                        rol.Add(r);
                    }
                }
            }
            return rol;
        }*/

    }
}