using Cursos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Cursos.BDConnection;


namespace Cursos.Controllers
{
    public class AccesoController : Controller
    {
        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;
      
        public ActionResult Login()
        {
            return View();
        }

        //Metodo de inicio de sesion
        [HttpPost]
        public ActionResult Login(usuarios credenciales)
        {
            List<usuarios> usuarios = new List<usuarios>();
            credenciales.pass = EncriptarSha256(credenciales.pass);

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_validaUsuario", cn);
                cmd.Parameters.AddWithValue("Email", credenciales.email);
                cmd.Parameters.AddWithValue("Pass", credenciales.pass);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        credenciales.id = dr.GetInt32(0);
                        credenciales.rol = dr.GetString(1);
                        credenciales.nombre = dr.GetString(2);
                        credenciales.apellido = dr.GetString(3);
                        credenciales.email = dr.GetString(4);
                        //credenciales.pass = dr.GetString(5);
                        credenciales.matricula = dr.IsDBNull(6) ? null : dr.GetString(6);
                        //credenciales.matricula= dr.GetString(6);
                        credenciales.carrera = dr.IsDBNull(7) ? null : dr.GetString(7);
                        //credenciales.carrera= dr.GetString(7);
                        credenciales.estudios = dr.IsDBNull(8) ? null : dr.GetString(8);
                        //credenciales.estudios= dr.GetString(8);
                        usuarios.Add(credenciales);
                    }
                }
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
            return RedirectToAction("Login", "Acceso");
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

        public ActionResult Error()
        {
            return View();
        }
    }
}