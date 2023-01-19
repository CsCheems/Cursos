using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Web.Services.Description;
using Cursos.BDConnection;

namespace Cursos.Models
{
    public class ViewModelCursoUsuario
    {
        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;

        public IEnumerable<usuarios> usuario { get; set; }
        public IEnumerable<cursos> curso { get; set; }
        public IEnumerable<estatus> estado { get; set; }

        public bool setPendientePago(int cid)
        {
            bool registrado;
            string mensaje;
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
        }

        //Metodo para despligar cursos adquiridos por usuario
        public List<cursos> GetMisCursos()
        {
            List<cursos> listCursos = new List<cursos>();
            usuarios u = (usuarios)HttpContext.Current.Session["usuario"];
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_obtenCursosUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("IdUsuario", u.id);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        cursos c = new cursos();
                        modalidad m = new modalidad();
                        c.id = dr.GetInt32(0);
                        c.nombre = dr.GetString(1);
                        m.modalidad1 = dr.GetString(2);
                        c.modalidad1 = m;
                        c.lugar = dr.GetString(3);
                        c.horas = dr.GetInt32(4);
                        c.urlTemario = dr.GetString(5);
                        listCursos.Add(c);
                    }
                }

            }
            return listCursos;
        }

    }

    

}