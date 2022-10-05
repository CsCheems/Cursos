using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cursos.Models
{
    public class ViewModelUsuario
    {

        //static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq; user id=adminedco; pwd=edco_uteq_2022**";
        static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";

        public IEnumerable<usuarios> usuario { get; set; }
        public IEnumerable<rol> roles { get; set; }

        public List<usuarios> GetUsuarios()
        {
            List<usuarios> usuarios = new List<usuarios>();
            List<rol> rol = new List<rol>();

            string sql = "select usuarios.id, usuarios.nombre, usuarios.apellido, rol.id, rol.rolUser, usuarios.email, usuarios.pass from usuarios inner join rol on rol.id = usuarios.rol;";
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

        public List<rol> GetRoles()
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
        }

    }
}