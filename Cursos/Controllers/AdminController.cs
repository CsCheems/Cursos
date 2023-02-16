using Cursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Dynamic;
using System.Data.Entity.Infrastructure;
using Cursos.Permisos;
using Cursos.BDConnection;

namespace Cursos.Controllers
{
    [ValidarSesionAdmin]
    public class AdminController : Controller
    {
        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;

        [HttpGet]
        public ActionResult Tablas()
        {
            List<cursos> obj = new List<cursos>();
            string sql = "select * from cursos;";
            //string sql = "select * from cursos;";

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using(SqlDataReader dr = cmd.ExecuteReader())
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
                            obj.Add(c);
                             
                        }
                    }
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult TablasUsuarios()
        {
            List<usuarios> usuario = new List<usuarios>();
            string sql = "select * from usuarios;";
            
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            usuarios u = new usuarios();
                            
                            u.id = dr.GetInt32(0);
                            u.rol = dr.GetString(1);
                            u.nombre = dr.GetString(2);
                            u.apellido = dr.GetString(3);
                            u.email = dr.GetString(4);
                            u.pass = dr.GetString(5);
                            u.matricula = dr.IsDBNull(6) ? null : dr.GetString(6);
                            u.carrera = dr.IsDBNull(7) ? null : dr.GetString(7);
                            u.estudios = dr.IsDBNull(8) ? null : dr.GetString(8);
                            usuario.Add(u);
                        }
                    }
                }
            }
            return View(usuario);
        }

        public ActionResult Pagos()
        {
            cursosUsuario cu = new cursosUsuario();
            dynamic dynModel = new ExpandoObject();
            dynModel.curso = cu.getPendientePago();
            return View(dynModel);
        }

        [HttpPost]
        public ActionResult Pagos(int id)
        {
            try
            {
                cursosUsuario cu = new cursosUsuario();
                cu.validarPago(id);
                return RedirectToAction("Pagos", "Admin");
            }
            catch
            {
                return View();
            }
        }
    }
}