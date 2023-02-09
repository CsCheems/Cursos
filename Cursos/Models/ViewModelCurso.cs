using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Cursos.BDConnection;

namespace Cursos.Models
{
    public class ViewModelCurso
    {
        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;

        public IEnumerable<cursos> cursos { get; set; }

        public List<cursos> GetCursos()
        {
            List<cursos> listCursos = new List<cursos>();
            List<modalidad> listModalidad = new List<modalidad>();
            string sql = "select * from cursos;";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        cursos c = new cursos();
                        modalidad m = new modalidad();
                        c.id = dr.GetInt32(0);
                        c.nombre = dr.GetString(1);
                        c.modalidad = dr.GetString(2);
                        c.lugar = dr.GetString(3);
                        c.horas = dr.GetInt32(4);
                        c.costo = dr.GetDecimal(5);
                        c.costoPref = dr.GetDecimal(6);
                        c.urlTemario = dr.GetString(7);
                        c.requisitos = dr.GetString(8);
                        c.criterioEval = dr.GetString(9);
                        c.imgUrl = dr.GetString(10);
                        listCursos.Add(c);
                    }
                }

            }
            return listCursos;
        }

        /*public List<modalidad> GetMod()
        {
            List<modalidad> listModalidad = new List<modalidad>();
            string sql = "select * from modalidad;";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        modalidad m = new modalidad();
                        m.id = dr.GetInt32(0);
                        m.modalidad1 = dr.GetString(1);
                        listModalidad.Add(m);
                    }
                }

            }
            return listModalidad;
        }*/

    }


}