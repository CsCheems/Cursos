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
    public class ViewModelUsuarioCurso
    {
        public int id { get; set; }
        //CURSO
        public int idCurso { get; set; }
        public string nombreCurso { get; set; }
        public int idModalidad { get; set; }
        public string lugar { get; set; }
        public int horas { get; set; }
        public DateTime fechaIni { get; set; }
        public DateTime fechaTer { get; set; }
        public decimal costo { get; set; }
        public decimal costoPref { get; set; }
        public string urlTemario { get; set; }
        public string requisitos { get; set; }
        public string criterioEval { get; set; }
        public string imgUrl { get; set; }
        public string modalidad1 { get; set; }
        

        //USUARIO
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public string passConfirm { get; set; }
        public bool estudiante { get; set; }
        public string documento { get; set; }
        public int sexo_id { get; set; }
        public int rol_id { get; set; }

        //ESTATUS
        public int idEstatus { get; set; }
        public string dIdUsuario { get; set; }
        public string comprobantePago { get; set; }
    }
}