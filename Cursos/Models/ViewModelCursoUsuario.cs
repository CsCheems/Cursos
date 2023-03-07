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
        //private int numUsuarios;

        public IEnumerable<usuario> usuario { get; set; }
        public IEnumerable<curso> curso { get; set; }
        public IEnumerable<estatus> estado { get; set; }

        public bool setPendientePago()
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
                usuario u = (usuario)HttpContext.Current.Session["usuario"];
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {

                    SqlCommand cmd = new SqlCommand("SP_registraCursoUsuario", cn);
                    /*cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdUsuario", u.id);
                    cmd.Parameters.AddWithValue("IdCurso", cid);
                    cmd.Parameters.AddWithValue("factura", )
                    cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;*/


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



        public List<usuario> getUsuariosXCursos()
        { 
            List<usuario> listUsus= new List<usuario>();
            string sql = "SELECT cursosUsuario.idUsuario, usuarios.nombre, usuarios.apellido, usuarios.email, usuarios.matricula, usuarios.carrera, usuarios.estudios FROM cursosUsuario INNER JOIN usuarios ON usuarios.id = cursosUsuario.idUsuario";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                using(SqlDataReader dr = cmd.ExecuteReader()){
                    while (dr.Read())
                    {
                        usuario u = new usuario();
                        u.id = dr.GetInt32(0);
                        u.nombre = dr.GetString(1);
                        u.apellido= dr.GetString(2);
                        u.email = dr.GetString(3);
                        
                        listUsus.Add(u);
                    }
                    cn.Close();
                }
                
            }
            return listUsus;
        }

        //Metodo para despligar cursos adquiridos por usuario
        public List<curso> GetMisCursos()
        {
            List<curso> listCursos = new List<curso>();
            usuario u = (usuario)HttpContext.Current.Session["usuario"];
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