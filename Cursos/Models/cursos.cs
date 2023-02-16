namespace Cursos.Models
{
    using Cursos.BDConnection;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Services.Description;

    public partial class cursos
    {
        public int id { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(20)]
        public String modalidad { get; set; }

        [StringLength(255)]
        public string lugar { get; set; }

        public int? horas { get; set; }

        public DateTime fechaIni { get; set; }

        public DateTime fechaTer { get; set; }

        public decimal? costo { get; set; }

        public decimal? costoPref { get; set; }

        [StringLength(255)]
        public string urlTemario { get; set; }

        [StringLength(100)]
        public string requisitos { get; set; }

        [StringLength(100)]
        public string criterioEval { get; set; }

        [StringLength(255)]
        public string imgUrl { get; set; }

        //Metodos
        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;

        //Metodo para obtener datos del curso
        public List<cursos> GetCursos()
        {
            List<cursos> listCursos = new List<cursos>();
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
                        c.id = dr.GetInt32(0);
                        c.nombre = dr.GetString(1);
                        c.modalidad = dr.GetString(2);
                        c.lugar = dr.GetString(3);
                        c.horas = dr.GetInt32(4);
                        c.fechaIni = dr.GetDateTime(5);
                        c.fechaTer = dr.GetDateTime(6);
                        c.costo = dr.GetDecimal(7);
                        c.costoPref = dr.GetDecimal(8);
                        c.urlTemario = dr.GetString(9);
                        c.requisitos = dr.GetString(10);
                        c.criterioEval = dr.GetString(11);
                        c.imgUrl = dr.GetString(12);
                        listCursos.Add(c);
                    }
                }
                
            }
            return listCursos;
        }

        //Metodo para agregar cursos
        public bool agregarCurso(cursos cursosInfo)
        {
            bool registrado;
            string mensaje;
            int i;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_registraCurso", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Nombre", cursosInfo.nombre);
                cmd.Parameters.AddWithValue("Modalidad", cursosInfo.modalidad);
                cmd.Parameters.AddWithValue("Lugar", cursosInfo.lugar);
                cmd.Parameters.AddWithValue("Horas", cursosInfo.horas);
                cmd.Parameters.AddWithValue("FechaIni", cursosInfo.fechaIni);
                cmd.Parameters.AddWithValue("FechaTer", cursosInfo.fechaTer);
                cmd.Parameters.AddWithValue("Costo", cursosInfo.costo);
                cmd.Parameters.AddWithValue("CostoPref", cursosInfo.costoPref);
                cmd.Parameters.AddWithValue("UrlTemario", cursosInfo.urlTemario);
                cmd.Parameters.AddWithValue("Requisitos", cursosInfo.requisitos);
                cmd.Parameters.AddWithValue("CriterioEval", cursosInfo.criterioEval);
                cmd.Parameters.AddWithValue("ImgUrl", cursosInfo.imgUrl);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                

                cn.Open();
                i = cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
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


        //Metodo para editar cursos
        public bool editaCurso(cursos cursosInfo)
        {
            bool editado;
            string mensaje;
            int i;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_editaCurso", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Nombre", cursosInfo.nombre);
                cmd.Parameters.AddWithValue("Modalidad", cursosInfo.modalidad);
                cmd.Parameters.AddWithValue("Lugar", cursosInfo.lugar);
                cmd.Parameters.AddWithValue("Horas", cursosInfo.horas);
                cmd.Parameters.AddWithValue("FechaIni", cursosInfo.fechaIni);
                cmd.Parameters.AddWithValue("FechaTer", cursosInfo.fechaTer);
                cmd.Parameters.AddWithValue("Costo", cursosInfo.costo);
                cmd.Parameters.AddWithValue("CostoPref", cursosInfo.costoPref);
                cmd.Parameters.AddWithValue("UrlTemario", cursosInfo.urlTemario);
                cmd.Parameters.AddWithValue("Requisitos", cursosInfo.requisitos);
                cmd.Parameters.AddWithValue("CriterioEval", cursosInfo.criterioEval);
                cmd.Parameters.AddWithValue("ImgUrl", cursosInfo.imgUrl);
                cmd.Parameters.Add("Editado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cn.Open();
                i = cmd.ExecuteNonQuery();

                editado = Convert.ToBoolean(cmd.Parameters["Editado"].Value);
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

        //Metodo para eliminar curso
        public bool eliminarCurso(int id)
        {
            int i;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_eliminaCurso", cn);
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
