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
    public class ViewModelUsuarioEstudiante
    {
        public List<usuario> user { get; set; }
        public List<estudiante> student { get; set; }
    }
}