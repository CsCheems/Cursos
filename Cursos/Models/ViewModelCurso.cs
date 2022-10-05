using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Cursos.Models
{
    public class ViewModelCurso
    {

        //static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq; user id=adminedco; pwd=edco_uteq_2022**";
        static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";

        public IEnumerable<cursos> cursos { get; set; }
        public IEnumerable<modalidad> mod { get; set; }

        public List<cursos> GetCursos()
        {
            List<cursos> listCursos = new List<cursos>();
            List<modalidad> listModalidad = new List<modalidad>();
            string sql = "select cursos.id, cursos.nombre, cursos.modalidad, modalidad.id, modalidad.modalidad, cursos.lugar, cursos.horas, cursos.costo, cursos.costoPref, cursos.urlTemario, cursos.requisitos, cursos.criterioEval, cursos.imgUrl from cursos inner join modalidad on modalidad.id = cursos.modalidad;";
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
                        c.modalidad = dr.GetInt32(2);
                        m.id = dr.GetInt32(3);
                        m.modalidad1 = dr.GetString(4);
                        c.modalidad1 = m;
                        c.lugar = dr.GetString(5);
                        c.horas = dr.GetInt32(6);
                        c.costo = dr.GetDecimal(7);
                        c.costoPref = dr.GetDecimal(8);
                        c.urlTemario = dr.GetString(9);
                        c.requisitos = dr.GetString(10);
                        c.criterioEval = dr.GetString(11);
                        listCursos.Add(c);
                    }
                }

            }
            return listCursos;
        }

        public List<modalidad> GetMod()
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
        }

    }


}