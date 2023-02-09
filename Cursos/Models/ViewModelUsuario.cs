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
        public IEnumerable<usuarios> usuario { get; set; }
        //public IEnumerable<rol> roles { get; set; }
        public List<usuarios> GetCurrentUsuario()
        {
            List<usuarios> usuarios = new List<usuarios>();
            usuarios usuario = (usuarios)HttpContext.Current.Session["usuario"];
            string sql = "select * from usuarios where usuarios.id = " +usuario.id +";";
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
                        usuarios.Add(u);
                    }
                }
            }
            return usuarios;
        }

        public List<usuarios> GetUsuarios()
        {
            List<usuarios> usuarios = new List<usuarios>();
            

            string sql = "select * from usuarios;";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
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
                        u.matricula = dr.GetString(6);
                        u.carrera = dr.GetString(7);
                        u.estudios = dr.GetString(8);
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