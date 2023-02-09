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
using Cursos.Permisos;
using System.Net;

namespace Cursos.Controllers
{
    
    public class UsuarioController : Controller
    {

        //*****************REGISTRA USUARIO****************************


        public ActionResult InsertaUsuario()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult InsertaUsuario(usuarios umodel)
        {
            try
            {
                usuarios udb = new usuarios();
                if (udb.InsertaUsuario(umodel))
                {
                    ViewBag.Message = "Se agrego el usuario";
                    ModelState.Clear();
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
        [ValidarSesionAdmin]
        public ActionResult EditaUsuario(int id)
        {
            if(id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ViewModelUsuario vm = new ViewModelUsuario();
                dynamic dynModel = new ExpandoObject();
                dynModel.usuario = vm.GetUsuarios().Find(umodel => umodel.id == id);
                return View(dynModel);
            } 
        }

        [HttpPost]
        public ActionResult EditaUsuario(usuarios umodel)
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
        [ValidarSesionAdmin]
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

        //*****************CURSOS USUARIO****************************
        public ActionResult CursosUsuario()
        {
            ViewModelCursoUsuario vmcu = new ViewModelCursoUsuario();
            dynamic dynModel = new ExpandoObject();
            dynModel.cursos = vmcu.GetMisCursos();
            return View(dynModel);
        }

    }
}