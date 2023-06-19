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
using System.IO;
using System.Security.Policy;

namespace Cursos.Controllers
{
    [ValidarSesionAdmin]
    public class AdminController : Controller
    {
        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;

        [HttpGet]
        public ActionResult Tablas()
        {
            List<curso> obj = new List<curso>();
            string sql = "select curso.id, curso.nombre, curso.idModalidad, modalidad.id, " +
                "modalidad.modalidad, curso.lugar, curso.horas, curso.fechaIni, curso.fechaTer, curso.costo, curso.costoPref, " +
                "curso.urlTemario, curso.requisitos, curso.criterioEval, curso.imgUrl from curso " +
                "inner join modalidad on modalidad.id = curso.idModalidad;";


            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using(SqlDataReader dr = cmd.ExecuteReader())
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
            List<usuario> usuario = new List<usuario>();
            string sql = "select usuario.id, usuario.nombre, usuario.apellido, usuario.telefono, usuario.email, usuario.pass, " +
                "usuario.estudiante, usuario.documentoID, usuario.sexo_id, usuario.rol_id, " +
                "sexo.id, sexo.sexo, roles.id, roles.rol, estudiante.matricula, estudiante.carrera, " +
                "estudiante.nivelEstudios, estudiante.usuario_id from usuario " +
                "left join sexo on sexo.id = usuario.sexo_id " +
                "left join roles on roles.id = usuario.rol_id " +
                "left join estudiante on estudiante.usuario_id = usuario.id";
            
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            usuario credenciales = new usuario();
                            sexo s = new sexo();
                            roles r = new roles();
                            credenciales.id = dr.GetInt32(0);
                            credenciales.nombre = dr.GetString(1);
                            credenciales.apellido = dr.GetString(2);
                            credenciales.telefono = dr.GetString(3);
                            credenciales.email = dr.GetString(4);
                            credenciales.pass = dr.GetString(5);
                            credenciales.estudiante = dr.GetBoolean(6);
                            credenciales.sexo_id = dr.GetInt32(8);
                            credenciales.rol_id = dr.GetInt32(9);
                            s.id = dr.GetInt32(10);
                            s.sexo1=dr.GetString(11);
                            r.id = dr.GetInt32(12);
                            r.rol = dr.GetString(13);
                            credenciales.roles = r;
                            credenciales.sexo= s;
                            usuario.Add(credenciales);
                        }
                    }
                }
            }
            return View(usuario);
        }


        [HttpGet]
        public ActionResult Pagos()
        {

            List<cursoUsuario> userCourse= new List<cursoUsuario>();
          
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_pendientePago", cn);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        cursoUsuario cu = new cursoUsuario();
                        curso c = new curso();
                        usuario u = new usuario();
                        estatus e = new estatus();
                        cu.id = dr.GetInt32(0);
                        cu.idCurso = dr.GetInt32(1);
                        c.nombre= dr.GetString(2);
                        cu.curso = c;
                        u.email= dr.GetString(3);
                        cu.usuario = u;
                        e.estado= dr.GetString(4);
                        e.id = dr.GetInt32(5);
                        cu.estatus = e;
                        int fac = dr.GetInt32(6);
                        if(fac == 0) 
                        {
                            cu.factura = false;
                        }
                        else
                        {
                            cu.factura = true;
                        }
                        userCourse.Add(cu);
                    }
                }
            }
            return View(userCourse);
        }

        public FileResult verPdf(int id)
        {
            string hash ="";
            string sql = "SELECT comprobantePago FROM cursoUsuario WHERE id = " + id;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        hash = dr.GetString(0);
                    } 
                } 
            }
            var rutaArchivo = Path.Combine(Server.MapPath("~/Archivos/Comprobantes"), hash + ".pdf");
            return File(rutaArchivo, "application/pdf");
        }

        [HttpPost]
        public ActionResult Pagos(int id)
        {
            int i;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_validaPago", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", id);

                cn.Open();
                i = cmd.ExecuteNonQuery();
                cn.Close();
            }
            if (i >= 1)
            {
                return View();
            }
            else
            {
                return View();
            }
        }


        [ValidarSesion]
        public ActionResult AdministrarImagenes()
        {
            return View();
        }

        //*****************ADMIN IMAGENES****************************
        [HttpPost]
        public ActionResult AdministrarImagenes(HttpPostedFileBase img1, HttpPostedFileBase img2, HttpPostedFileBase img3) 
        {
            if (img1 != null && img1.ContentLength > 0)
            {
                if (img1.ContentLength > 1048576)
                {
                    ViewBag.Message = "La imagen 1 no debe exceder de 1MB.";
                    return View();
                }

                try
                {
                    string nombre = Path.GetFileName(img1.FileName);
                    MemoryStream ms = new MemoryStream();
                    img1.InputStream.CopyTo(ms);
                    byte[] data = ms.ToArray();

                    using (SqlConnection cn = new SqlConnection(cadenaConexion))
                    {
                        string query = "INSERT INTO imagenesFront (nombre, imagen) VALUES(@nombre, @imagen)";
                        SqlCommand cmd = new SqlCommand(query, cn);

                        // Asigna los valores a los parámetros
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@imagen", data);
                        cmd.CommandType = CommandType.Text;
                        // Abre la conexión y ejecuta la consulta
                        cn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    return View();
                } 
            }

            if (img2 != null && img2.ContentLength > 0)
            {
                if (img2.ContentLength > 1048576)
                {
                    ViewBag.Message = "La imagen 2 no debe exceder de 1MB.";
                    return View();
                }
                try
                {
                    string nombre = Path.GetFileName(img2.FileName);
                    MemoryStream ms = new MemoryStream();
                    img2.InputStream.CopyTo(ms);
                    byte[] data = ms.ToArray();

                    using (SqlConnection cn = new SqlConnection(cadenaConexion))
                    {
                        string query = "INSERT INTO imagenesFront (nombre, imagen) VALUES(@nombre, @imagen)";
                        SqlCommand cmd = new SqlCommand(query, cn);

                        // Asigna los valores a los parámetros
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@imagen", data);
                        cmd.CommandType = CommandType.Text;
                        // Abre la conexión y ejecuta la consulta
                        cn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    return View();
                }
                
            }

            if (img3 != null && img3.ContentLength > 0)
            {
                if (img3.ContentLength > 1048576)
                {
                    ViewBag.Message = "La imagen 3 no debe exceder de 1MB.";
                    return View();
                }
                try
                {
                    string nombre = Path.GetFileName(img3.FileName);
                    MemoryStream ms = new MemoryStream();
                    img3.InputStream.CopyTo(ms);
                    byte[] data = ms.ToArray();

                    using (SqlConnection cn = new SqlConnection(cadenaConexion))
                    {
                        string query = "INSERT INTO imagenesFront (nombre, imagen) VALUES(@nombre, @imagen)";
                        SqlCommand cmd = new SqlCommand(query, cn);

                        // Asigna los valores a los parámetros
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@imagen", data);
                        cmd.CommandType = CommandType.Text;
                        // Abre la conexión y ejecuta la consulta
                        cn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    return View();
                }
              
            }

            return RedirectToAction("Index", "Home");
        }

    }
}