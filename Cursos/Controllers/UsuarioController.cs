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
using System.Data.Entity.Infrastructure;
using System.Dynamic;

namespace Cursos.Controllers
{
    public class UsuarioController : Controller
    {
        //static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Database=edcouteq; user id=adminedco; pwd=edco_uteq_2022**";
        static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";


        //*****************REGISTRA USUARIO****************************
        public ActionResult InsertaUsuario()
        {
            var r = ModeloRol();
            return View(r);
        }

        public List<rol> ModeloRol()
        {
            List<rol> role = new List<rol>();
            string sql = "select * from rol";

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
            return role;
        }
        
        [HttpPost]
        public ActionResult InsertaUsuario(usuarios umodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    usuarios udb = new usuarios();
                    if (udb.InsertaUsuario(umodel))
                    {
                        ViewBag.Message = "Se agrego el usuario";
                        ModelState.Clear();
                    }
                }
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                else
                {
                    return RedirectToAction("TablasUsuarios", "Admin");
                }
               
            }
            catch
            {
                return View();
            }
        }

        //*****************EDITA USUARIO****************************
        
        public ActionResult EditaUsuario(int id)
        {
            ViewModelUsuario vm = new ViewModelUsuario();
            dynamic dynModel = new ExpandoObject();
            dynModel.usuario = vm.GetUsuarios().Find(umodel => umodel.id == id);
            dynModel.roles = vm.GetRoles();
            return View(dynModel);
        }

        [HttpPost]
        public ActionResult EditaUsuario(int id, usuarios umodel)
        {
            try
            {
                usuarios udb = new usuarios();
                udb.EditarUsuario(umodel);
                return RedirectToAction("TablasUsuarios", "Admin");
            }
            catch
            {
                return View();
            }  
        }


        //*****************ELIMINA USUARIO****************************

        public ActionResult EliminaUsuario(int id)
        {
            try
            {
                usuarios u = new usuarios();
                if (u.eliminarUsuario(id))
                {
                    ViewBag.AlertMsg = "El usuario se ha eliminado";
                }
                return RedirectToAction("TablasUsuarios", "Admin");
            }
            catch
            {
                return View();
            }
        }

        //*****************ENCRIPTADOR****************************

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