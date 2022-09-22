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

namespace Cursos.Controllers
{
    public class AdminController : Controller
    {
        //static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq;Integrated Security=true;";
        static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";
        
        public ActionResult InsertaUsuario()
        {
            List<rol> role = new List<rol>();
            string sql = "select * from rol;";

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            rol r = new rol();
                            r.id = dr.GetInt32(0);
                            r.rolUser = dr.GetString(1);
                            role.Add(r);
                        }
                    }
                }
            }
            return View(role);
            
        }

        [HttpGet]
        public ActionResult Tablas()
        {
            List<cursos> curso = new List<cursos>();
            //string sql = "select cursos.id, cursos.nombre, modalidad.modalidad, cursos.lugar, cursos.horas, cursos.costo, cursos.costoPref, cursos.urlTemario, cursos.requisitos, cursos.criterioEval, cursos.imgUrl from cursos inner join modalidad on modalidad.id = cursos.modalidad;";
            string sql = "select * from cursos;";

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
                            c.modalidad = dr.GetInt32(2);
                            //c.modalidad1.modalidad1 = dr.GetString(2);
                            c.lugar = dr.GetString(3);
                            c.horas = dr.GetInt32(4);
                            c.costo = dr.GetDecimal(5);
                            c.costoPref = dr.GetDecimal(6);
                            c.urlTemario = dr.GetString(7);
                            c.requisitos = dr.GetString(8);
                            c.criterioEval = dr.GetString(9);
                            curso.Add(c);
                        }
                    }
                }
            }
            return View(curso);
        }

        [HttpGet]
        public ActionResult TablasUsuarios()
        {
            List<usuarios> usuario = new List<usuarios>();
            //string sql = "select cursos.id, cursos.nombre, modalidad.modalidad as modalidad, cursos.lugar, cursos.horas, cursos.costo, cursos.costoPref, cursos.urlTemario, cursos.requisitos, cursos.criterioEval, cursos.imgUrl from cursos inner join modalidad on modalidad.id = cursos.modalidad;";
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
                            u.rol = dr.GetInt32(1);
                            u.nombre = dr.GetString(2);
                            u.apellido = dr.GetString(3);
                            u.email = dr.GetString(4);
                            usuario.Add(u);
                        }
                    }
                }
            }
            return View(usuario);
        }

        //Metodo para insertar usuario
        [HttpPost]
        public ActionResult InsertaUsuario(usuarios usuarioInfo)
        {
            bool registrado;
            string mensaje;
            if (usuarioInfo.pass == usuarioInfo.passConfirm)
            {
                usuarioInfo.pass = EncriptarSha256(usuarioInfo.pass);

            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_registraUsuario", cn);
                cmd.Parameters.AddWithValue("Rol", usuarioInfo.rol);
                cmd.Parameters.AddWithValue("Nombre", usuarioInfo.nombre);
                cmd.Parameters.AddWithValue("Apellido", usuarioInfo.apellido);
                cmd.Parameters.AddWithValue("Email", usuarioInfo.email);
                cmd.Parameters.AddWithValue("Pass", usuarioInfo.pass);
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
                return RedirectToAction("TablasUsuarios");
            }
            else
            {
                return View();
            }
        }

        //Metodo de inicio de sesion
        [HttpPost]
        public ActionResult Login(usuarios credenciales)
        {
            credenciales.pass = EncriptarSha256(credenciales.pass);

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_validaUsuario", cn);
                cmd.Parameters.AddWithValue("Email", credenciales.email);
                cmd.Parameters.AddWithValue("Pass", credenciales.pass);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                credenciales.id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }

            if (credenciales.id != 0)
            {
                Session["usuario"] = credenciales;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no encontrado";
                return View();
            }
        }

        //Metodo para cerrar sesion
        public ActionResult Logout()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login", "Admin");
        }

        //Metodo para encriptar password
        public static string EncriptarSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}