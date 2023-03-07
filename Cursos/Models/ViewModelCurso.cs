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

        public IEnumerable<curso> curso { get; set; }

        public List<curso> GetCursos()
        {
            List<curso> listCursos = new List<curso>();
            string sql = "select curso.id, curso.nombre, curso.idModalidad, modalidad.id, modalidad.modalidad, curso.lugar, curso.horas, " +
                "curso.fechaIni, curso.fechaTer, curso.costo, curso.costoPref, curso.urlTemario, curso.requisitos, curso.criterioEval, curso.imgUrl from curso " +
                "inner join modalidad on modalidad.id = curso.idModalidad;";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        curso c = new curso();
                        modalidad m = new modalidad();
                        c.id = dr.GetInt32(0);
                        c.nombre = dr.GetString(1);
                        c.idModalidad = dr.GetInt32(2);
                        m.id = dr.GetInt32(3);
                        m.modalidad1 = dr.GetString(4);
                        c.modalidad = m;
                        c.lugar = dr.GetString(5);
                        c.horas = dr.GetInt32(6);
                        c.fechaIni = dr.GetDateTime(7);
                        c.fechaTer = dr.GetDateTime(8);
                        c.costo = dr.GetDecimal(9);
                        c.costoPref = dr.GetDecimal(10);
                        c.urlTemario = dr.GetString(11);
                        c.requisitos = dr.GetString(12);
                        c.criterioEval = dr.GetString(13);
                        c.imgUrl = dr.IsDBNull(14) ? null : dr.GetString(14);
                        listCursos.Add(c);
                    }
                }

            }
            return listCursos;
        }
    }
}