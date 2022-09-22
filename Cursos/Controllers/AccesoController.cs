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

namespace Cursos.Controllers
{
    public class AccesoController : Controller
    {
        //static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";
        static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq;Integrated Security=true;";

        public ActionResult Login()
        {
            return View();
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
    }
}