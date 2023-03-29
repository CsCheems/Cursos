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

        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public string passConfirm { get; set; }
        public bool estudiante { get; set; }
        public string documento { get; set; }
        public int sexo_id { get; set; }
        public int rol_id { get; set; }
        public int idEstudiante { get; set; }
        public string matricula { get; set; }
        public string carrera { get; set; }
        public string nivelEstudios { get; set; }
        public int usuario_id { get; set; }
        public virtual roles roles { get; set; }
        public virtual sexo sexo { get; set; }
    }
}