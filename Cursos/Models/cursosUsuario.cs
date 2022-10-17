using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Cursos.Models
{
    public class cursosUsuario
    {
        static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq; user id=adminedco; pwd=edco_uteq_2022**";
        //static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";

        public int id { get; set; }

        public int idCurso { get; set; }

        public int idUsuario { get; set; }

        public int idEstatus { get; set; }

        public virtual estatus estado1 { get; set; }

        public virtual usuarios usuario { get; set; }

        public virtual cursos curso { get; set; }

        public bool setPendientePago(int cid)
        {
            int i;
            if (HttpContext.Current.Session["usuario"] == null)
            {
                return false;
            }
            else
            {
                usuarios u = (usuarios)HttpContext.Current.Session["usuario"];
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {

                    SqlCommand cmd = new SqlCommand("SP_registraCursoUsuario", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdUsuario", u.id);
                    cmd.Parameters.AddWithValue("IdCurso", cid);

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

        public List<cursosUsuario> getPendientePago()
        {
            List<cursosUsuario> lcu = new List<cursosUsuario>();
            int i;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_obtenPendientePago", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    cursosUsuario cu = new cursosUsuario();
                    cursos c = new cursos();
                    usuarios u = new usuarios();
                    estatus e = new estatus();
                    while (dr.Read())
                    {
                        cu.id = dr.GetInt32(0);
                        cu.idCurso = dr.GetInt32(1);
                        c.nombre = dr.GetString(2);
                        cu.curso = c;
                        u.email = dr.GetString(3);
                        cu.usuario = u;
                        e.estatus1 = dr.GetString(4);
                        cu.estado1 = e;
                        lcu.Add(cu);                        
                    }
                }
               
            }
            return lcu;

        }

        //Metodo para registrar un usuario al curso
       public bool validarPago(int cid)
        {
            int i;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_validaPago", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", cid);

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