using System;
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

namespace Cursos.Controllers
{
    public class CursosController : Controller
    {
        //static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq; user id=adminedco; pwd=edco_uteq_2022**";
        static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";

        //*****************REGISTRA CURSO****************************

        public ActionResult RegistrarCurso()
        {
            var mod = ModeloMod();
            return View(mod); 
        }

        public List<modalidad> ModeloMod()
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
        }

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

        [HttpGet]
        public ActionResult EditaCurso(int id)
        {
            ViewModelCurso vm = new ViewModelCurso();
            dynamic dynModel = new ExpandoObject();
            dynModel.curso = vm.GetCursos().Find(cmodel => cmodel.id == id);
            dynModel.modalidad = vm.GetMod();

            return View(dynModel);
        }

        [HttpPost]
        public ActionResult EditaCurso(int id, cursos cmodel)
        {
            try
            {
                cursos cdb = new cursos();
                cdb.editaCurso(cmodel);
                return RedirectToAction("Tablas", "Admin");
            }
            catch
            {
                return View();
            }  
        }

        //*****************ELIMINA CURSO****************************

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
    }
}