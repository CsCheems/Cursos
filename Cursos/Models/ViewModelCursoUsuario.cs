using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Cursos.Models
{
    public class ViewModelCursoUsuario
    {
        //static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq; user id=adminedco; pwd=edco_uteq_2022**";
        static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";

        public IEnumerable<usuarios> usuario { get; set; }
        public IEnumerable<cursos> curso { get; set; }


        //Metodo para un usuario al curso
        public bool registraCursoUsuario(int cid)
        {
            int i;
            if(HttpContext.Current.Session["usuario"] == null)
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

    }

    

}