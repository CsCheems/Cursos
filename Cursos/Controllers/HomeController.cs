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
using System.Dynamic;

namespace Cursos.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            ViewModelCurso vm = new ViewModelCurso();
            dynamic dynModel = new ExpandoObject();
            dynModel.curso = vm.GetCursos();
            dynModel.modalidad = vm.GetMod();

            return View(dynModel);
        }

        public ActionResult CursoDetalle(int id)
        {
            ViewModelCurso vm = new ViewModelCurso();
            dynamic dynModel = new ExpandoObject();
            dynModel.curso = vm.GetCursos().Find(cmodel => cmodel.id == id);
            dynModel.modalidad = vm.GetMod();

            return View(dynModel);
        }

        public ActionResult CursoFormulario()
        {
            ViewModelCurso vmc = new ViewModelCurso();
            dynamic dynModel = new ExpandoObject();
            dynModel.modalidad = vmc.GetMod();
            return View(dynModel);
        }
        
    }
}