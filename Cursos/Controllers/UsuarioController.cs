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
    public class UsuarioController : Controller
    {
        //static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq;Integrated Security=true;";
        static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";

        
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
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        public static string EncriptarSha256(String texto)
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