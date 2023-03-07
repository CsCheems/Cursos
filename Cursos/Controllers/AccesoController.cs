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
        public ActionResult Login(usuario credenciales)
        {
            List<usuario> user = new List<usuario>();
            credenciales.pass = EncriptarSha256(credenciales.pass);

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_validaUsuario2", cn);
                cmd.Parameters.AddWithValue("Email", credenciales.email);
                cmd.Parameters.AddWithValue("Pass", credenciales.pass);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        
                        credenciales.id = dr.GetInt32(0);
                        credenciales.nombre = dr.GetString(1);
                        credenciales.apellido = dr.GetString(2);
                        credenciales.telefono= dr.GetString(3);
                        credenciales.email = dr.GetString(4);
                        credenciales.pass = dr.GetString(5);
                        credenciales.estudiante = dr.GetBoolean(6);
                        //credenciales.documento
                        credenciales.sexo_id= dr.GetInt32(8);
                        credenciales.rol_id= dr.GetInt32(9);
                        /*sexo s = new sexo();
                        s.id = dr.GetInt32(10);
                        s.sexo1 = dr.GetString(11);
                        credenciales.sexo = s;
                        roles r = new roles();
                        r.id = dr.GetInt32(12);
                        r.rol = dr.GetString(13);
                        credenciales.roles = r;
                        if (credenciales.estudiante == true)
                        {
                            estudiante e = new estudiante();
                            e.matricula = dr.IsDBNull(14) ? null : dr.GetString(14);
                            e.carrera = dr.IsDBNull(15) ? null : dr.GetString(15);
                            e.nivelEstudios = dr.IsDBNull(16) ? null : dr.GetString(16);
                            credenciales.estudiante1 = (ICollection<estudiante>)e;
                        }*/
                        user.Add(credenciales);
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