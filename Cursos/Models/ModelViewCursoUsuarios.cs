using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Web.Services.Description;
using Cursos.BDConnection;

namespace Cursos.Models
{
    public class ModelViewCursoUsuarios
    {
        public List<usuario> user { get; set; }
        public List<curso> course { get; set; }
        public List<estudiante> student { get;set; }
        public List<cursoUsuario> userCourse { get; set; }

    }
}