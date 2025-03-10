﻿using Cursos.Models;
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
            curso vm = new curso();
            Image img = new Image();
            dynamic dynModel = new ExpandoObject();
            dynModel.curso = vm.GetCursos();
            dynModel.img = img.GetImages();
            return View(dynModel);
        }

        public ActionResult CursoDetalle(int id)
        {
            curso vm = new curso();
            dynamic dynModel = new ExpandoObject();
            dynModel.curso = vm.GetCursos().Find(cmodel => cmodel.id == id);
            return View(dynModel);
        }

        /*public ActionResult CursoFormulario()
        {
            ViewModelCurso vmc = new ViewModelCurso();
            dynamic dynModel = new ExpandoObject();
            return View(dynModel);
        }*/
        
    }
}