using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cursos.Models;

namespace Cursos.Controllers
{
    public class CursosController : Controller
    {
        //static string cadenaConexion = "Data Source=DESKTOP-RDBRQG8;Initial Catalog=edcouteq;Integrated Security=true;";
        static string cadenaConexion = "Data Source=DESKTOP-ADDCRJO;Initial Catalog=edcouteq;Integrated Security=true; user id=sa; pwd=123";

        //*****************REGISTRA CURSO****************************

        public ActionResult RegistrarCurso()
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
            return View(mod); 
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
                return View();
            }
            catch 
            {
                return View();
            }
        }

        //*****************EDITA CURSO****************************

        [HttpGet]
        public ActionResult EditaCurso(int id)
        {
            cursos c = new cursos();
            return View(c.GetCursos().Find(cmodel => cmodel.id == id));
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