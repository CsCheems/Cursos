namespace Cursos.Models
{
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

        public int modalidad { get; set; }

        [StringLength(255)]
        public string lugar { get; set; }

        public int? horas { get; set; }

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

        public virtual modalidad modalidad1 { get; set; }

        //Metodos
        static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq;Integrated Security=true;";
        //static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";

        //Metodo para obtener datos del curso
        public List<cursos> getCursos()
        {
            List<cursos> listCursos = new List<cursos>();
            List<modalidad> listModalidad = new List<modalidad>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_obtenCurso", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                cn.Open();
                sd.Fill(dt);
                cn.Close();

                foreach(DataRow dr in dt.Rows)
                {
                    listCursos.Add(
                        new cursos
                        {
                            id = Convert.ToInt32(dr["id"]),
                            nombre = Convert.ToString(dr["nombre"]),
                            modalidad = Convert.ToInt32(dr["modalidad"]),
                            lugar = Convert.ToString(dr["lugar"]),
                            horas = Convert.ToInt32(dr["horas"]),
                            costo = Convert.ToInt32(dr["costo"]),
                            costoPref = Convert.ToInt32(dr["costoPref"]),
                            urlTemario = Convert.ToString(dr["urlTemario"]),
                            requisitos = Convert.ToString(dr["requisitos"]),
                            criterioEval = Convert.ToString(dr["criterioEval"]),
                            imgUrl = Convert.ToString(dr["imgUrl"])
                        });
                }
                return listCursos;
                
            }

           
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
                cmd.Parameters.AddWithValue("Nombre", cursosInfo.nombre);
                cmd.Parameters.AddWithValue("Modalidad", cursosInfo.modalidad);
                cmd.Parameters.AddWithValue("Lugar", cursosInfo.lugar);
                cmd.Parameters.AddWithValue("Horas", cursosInfo.horas);
                cmd.Parameters.AddWithValue("Costo", cursosInfo.costo);
                cmd.Parameters.AddWithValue("CostoPref", cursosInfo.costoPref);
                cmd.Parameters.AddWithValue("UrlTemario", cursosInfo.urlTemario);
                cmd.Parameters.AddWithValue("Requisitos", cursosInfo.requisitos);
                cmd.Parameters.AddWithValue("CriterioEval", cursosInfo.criterioEval);
                cmd.Parameters.AddWithValue("ImgUrl", cursosInfo.imgUrl);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

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
                cmd.Parameters.AddWithValue("Id", cursosInfo.id);
                cmd.Parameters.AddWithValue("Nombre", cursosInfo.nombre);
                cmd.Parameters.AddWithValue("Modalidad", cursosInfo.modalidad);
                cmd.Parameters.AddWithValue("Lugar", cursosInfo.lugar);
                cmd.Parameters.AddWithValue("Horas", cursosInfo.horas);
                cmd.Parameters.AddWithValue("Costo", cursosInfo.costo);
                cmd.Parameters.AddWithValue("CostoPref", cursosInfo.costoPref);
                cmd.Parameters.AddWithValue("UrlTemario", cursosInfo.urlTemario);
                cmd.Parameters.AddWithValue("Requisitos", cursosInfo.requisitos);
                cmd.Parameters.AddWithValue("CriterioEval", cursosInfo.criterioEval);
                cmd.Parameters.AddWithValue("ImgUrl", cursosInfo.imgUrl);
                cmd.Parameters.Add("Editado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                i = cmd.ExecuteNonQuery();

                editado = Convert.ToBoolean(cmd.Parameters["Editado"].Value);
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

        //Metodo para eliminar curso
        public bool eliminarCurso(cursos cursosInfo)
        {
            int i;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_eliminaCurso", cn);
                cmd.Parameters.AddWithValue("Id", cursosInfo.id);
                cmd.CommandType = CommandType.StoredProcedure;

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
