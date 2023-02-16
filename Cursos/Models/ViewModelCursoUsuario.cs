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
        private int numUsuarios;

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



        public List<usuarios> getUsuariosXCursos()
        { 
            List<usuarios> listUsus= new List<usuarios>();
            string sql = "SELECT cursosUsuario.idUsuario, usuarios.nombre, usuarios.apellido, usuarios.email, usuarios.matricula, usuarios.carrera, usuarios.estudios FROM cursosUsuario INNER JOIN usuarios ON usuarios.id = cursosUsuario.idUsuario";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                using(SqlDataReader dr = cmd.ExecuteReader()){
                    while (dr.Read())
                    {
                        usuarios u = new usuarios();
                        u.id = dr.GetInt32(0);
                        u.nombre = dr.GetString(1);
                        u.apellido= dr.GetString(2);
                        u.email = dr.GetString(3);
                        u.matricula= dr.IsDBNull(4) ? null : dr.GetString(4);
                        u.carrera = dr.IsDBNull(5) ? null : dr.GetString(5);
                        u.estudios = dr.IsDBNull(6) ? null : dr.GetString(6);
                        listUsus.Add(u);
                    }
                    cn.Close();
                }
                
            }
            return listUsus;
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

    }

    

}