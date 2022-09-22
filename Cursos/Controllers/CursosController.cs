using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cursos.Models;

namespace Cursos.Controllers
{
    public class CursosController : Controller
    {
        //static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq;Integrated Security=true;";
        static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";

        public ActionResult RegistrarCurso()
        {
            List<modalidad> mod = new List<modalidad>();
            string sql = "select * from modalidad;";

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            modalidad m = new modalidad();
                            m.id = dr.GetInt32(0);
                            m.modalidad1 = dr.GetString(1);
                            mod.Add(m);
                        }
                    }
                }
            }
            return View(mod);
        }

        public ActionResult EditaCurso()
        {
            return View();
        }

        //Metodo para registrar un curso
        [HttpPost]
        public ActionResult RegistrarCurso(cursos cursosInfo)
        {
            bool registrado;
            string mensaje;

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
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }

            ViewData["Mensaje"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Tablas", "Admin");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditaCurso(cursos cursosInfo)
        {
            bool editado;
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
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                cmd.ExecuteNonQuery();

                editado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
            }

            if (editado)
            {
                return RedirectToAction("Admin", "Cursos");
            }
            else
            {
                return View();
            }
        }
    }
}