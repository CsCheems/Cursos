﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cursos.Models;
using System.Dynamic;
using System.IO;
using System.Web.UI.WebControls;
using Cursos.Permisos;
using Cursos.BDConnection;

namespace Cursos.Controllers
{
   
    public class CursosController : Controller
    {
        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;

        //*****************REGISTRA CURSO****************************

        [ValidarSesionAdmin]
        public ActionResult RegistrarCurso()
        {
            return View(); 
        }

        /*public List<modalidad> ModeloMod()
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
            return mod;
        }*/

        
        [HttpPost]
        public ActionResult RegistrarCurso(cursos cmodel)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    cursos cdb = new cursos();
                    if (cdb.agregarCurso(cmodel))
                    {
                        ViewBag.Message = "Se agrego el curso";
                        ModelState.Clear();
                    }
                }
                return RedirectToAction("Tablas", "Admin");
            }
            catch 
            {
                return View();
            }
        }

        public void DirectorioExistente()
        {

            // si el directorio \Recursos\imgs no existe - create it. 
            if (!System.IO.Directory.Exists(Server.MapPath(@"~/Recursos/imgs")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(@"~/Recursos/imgs"));
            }
        }

        public ActionResult CargaArchivo()
        {
            DirectorioExistente();
            String fname;
            if(Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for(int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        fname = file.FileName;
                        fname = Path.Combine(Server.MapPath("~/Recursos/imgs"), fname);
                        file.SaveAs(fname);
                    }
                    return Json("Se cargo correctamente");
                }catch(Exception ex)
                {
                    return Json("Ocurrio un error. Detalles: " + ex.Message);
                }
            }
            else
            {
                return Json("No se cargo ningun archivo");
            }
        }


        //*****************EDITA CURSO****************************
        [ValidarSesionAdmin]
        [HttpGet]
        public ActionResult EditaCurso(int id)
        {
            ViewModelCurso vm = new ViewModelCurso();
            dynamic dynModel = new ExpandoObject();
            dynModel.curso = vm.GetCursos().Find(cmodel => cmodel.id == id);

            return View(dynModel);
        }

        [HttpPost]
        public ActionResult EditaCurso(cursos cmodel)
        {
            try
            {
                cursos cdb = new cursos();
                if (cdb.editaCurso(cmodel))
                {
                    ViewBag.AlertMsg = "El curso ha sido editado";
                }
                return RedirectToAction("Tablas", "Admin");
            }
            catch
            {
                return View();
            }  
        }

        //*****************ELIMINA CURSO****************************
        [ValidarSesionAdmin]
        public ActionResult EliminaCurso(int id)
        {
            try
            {
                cursos c = new cursos();
                if (c.eliminarCurso(id))
                {
                    ViewBag.AlertMsg = "El cursos se ha eliminado";
                }
                return RedirectToAction("Tablas", "Admin");
            }
            catch
            {
                return View();
            }
        }

        //*****************REGISTRA USUARIO A CURSO****************************


        public ActionResult RegistraCursoUsuario(int id)
        {
            ViewModelCursoUsuario vmcu = new ViewModelCursoUsuario();
            if (vmcu.setPendientePago(id))
            {
                ViewBag.AlertMsg = "El usuario se ha registrado al curso";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Acceso");
        }

        //*****************FICHA DE PAGO****************************
        [ValidarSesion]
        public ActionResult FichaDePago(int id)
        {
            cursos vc = new cursos();
            usuarios u = new usuarios();
            dynamic dynModel = new ExpandoObject();
            dynModel.curso = vc.GetCursos().Find(cmodel => cmodel.id == id);
            dynModel.usuario = u.GetUsuario();
            return View(dynModel);
        }

        //******************VER DETALLES DE CURSO*******************
        [ValidarSesionAdmin]
        [HttpGet]
        public ActionResult VerCursosDetalles()
        {
            cursos c = new cursos();
            ViewModelCursoUsuario vmcu = new ViewModelCursoUsuario();
            dynamic dynModel = new ExpandoObject();
            dynModel.cursoDet = c.GetCursos();
            return View(dynModel);
        }

        [ValidarSesionAdmin]
        public ActionResult VerUsuariosCurso(int id)
        {
            cursos c = new cursos();
            ViewModelCursoUsuario vmcu = new ViewModelCursoUsuario();
            dynamic dynModel = new ExpandoObject();
            dynModel.usuariosDet = vmcu.getUsuariosXCursos();
            return View(dynModel);
        }
    }
}