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
using System.IO;
using System.Xml.Linq;
using Cursos.BDConnection;
using System.Web.Services.Description;
using System.Reflection;

namespace Cursos.Controllers
{
    
    public class UsuarioController : Controller
    {

        //*****************REGISTRA USUARIO****************************

        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;

        public ActionResult InsertaUsuario()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult InsertaUsuario(usuario umodel, estudiante emodel)
        {

            bool registrado;
            string mensaje;
            int i;
            int id;
            if (umodel.pass == umodel.passConfirm)
            {
                umodel.pass = EncriptarSha256(umodel.pass);
            }
            else
            {
                return View();
            }

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_registraUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Nombre", umodel.nombre);
                cmd.Parameters.AddWithValue("Apellido", umodel.apellido);
                cmd.Parameters.AddWithValue("Telefono", umodel.telefono);
                cmd.Parameters.AddWithValue("Email", umodel.email);
                cmd.Parameters.AddWithValue("Pass", umodel.pass);
                if (umodel.estudiante == true)
                {
                    cmd.Parameters.AddWithValue("Estudiante", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("Estudiante", 0);
                }
                cmd.Parameters.AddWithValue("Sexo", umodel.sexo_id);
                cmd.Parameters.AddWithValue("Rol", umodel.rol_id);

                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cn.Open();
                i = cmd.ExecuteNonQuery();
                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                cn.Close();

            }

            string sql = "select id from usuario where email = '" + umodel.email + "';";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        id = dr.GetInt32(0);
                        umodel.id = id;
                    }
                }
                cn.Close();
            }

            if (i >= 1)
            {
                if (umodel.estudiante == true)
                {
                    InsertaEstudiante(umodel, emodel);
                }
                return RedirectToAction("TablasUsuarios", "Admin"); ;
            }
            else
            {
                return View();
            }



        }

        public ActionResult InsertaUsuario2(usuario umodel, estudiante emodel)
        {
            bool registrado;
            string mensaje;
            int i;
            int id;
            if (umodel.pass == umodel.passConfirm)
            {
                umodel.pass = EncriptarSha256(umodel.pass);
            }
            else
            {
                return RedirectToAction("Login", "Acceso");
            }

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_registraUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Nombre", umodel.nombre);
                cmd.Parameters.AddWithValue("Apellido", umodel.apellido);
                cmd.Parameters.AddWithValue("Telefono", umodel.telefono);
                cmd.Parameters.AddWithValue("Email", umodel.email);
                cmd.Parameters.AddWithValue("Pass", umodel.pass);
                if (umodel.estudiante == true)
                {
                    cmd.Parameters.AddWithValue("Estudiante", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("Estudiante", 0);
                }
                cmd.Parameters.AddWithValue("Sexo", umodel.sexo_id);
                cmd.Parameters.AddWithValue("Rol", umodel.rol_id);

                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cn.Open();
                i = cmd.ExecuteNonQuery();
                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                cn.Close();

            }

            string sql = "select id from usuario where email = '" + umodel.email + "';";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        id = dr.GetInt32(0);
                        umodel.id = id;
                    }
                }
                cn.Close();
            }

            if (i >= 1)
            {
                if (umodel.estudiante == true)
                {
                    InsertaEstudiante(umodel, emodel);
                }
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                return RedirectToAction("Login", "Acceso");
            }
        }

        //*****************REGISTRA ESTUDIANTE**********************

        public void InsertaEstudiante(usuario umodel, estudiante emodel)
        {
            bool registrado;
            string mensaje;
            int i;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
       
                SqlCommand cmd = new SqlCommand("SP_registraEstudiante", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Matricula", emodel.matricula);
                cmd.Parameters.AddWithValue("Carrera", emodel.carrera);
                cmd.Parameters.AddWithValue("Estudios", emodel.nivelEstudios);
                cmd.Parameters.AddWithValue("IdUsuario", umodel.id);

                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cn.Open();
                i = cmd.ExecuteNonQuery();
                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                cn.Close();

            }
            
        }

        //*****************EDITA USUARIO****************************
        [ValidarSesionAdmin]
        [HttpGet]
        public ActionResult EditaUsuario(int id)
        {
            List<usuario> user = new List<usuario>();
            string sql = "select usuario.id, usuario.rol_id, roles.id, roles.rol, usuario.nombre, usuario.apellido, " +
                         "usuario.sexo_id, sexo.id, sexo.sexo, usuario.telefono, usuario.email, " +
                         "usuario.estudiante " +
                         "from usuario " +
                         "inner join sexo on sexo.id = usuario.sexo_id " +
                         "inner join roles on roles.id = usuario.rol_id " +
                         "where usuario.id = " + id + ";";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usuario u = new usuario();
                        roles r = new roles();
                        sexo s =new sexo();
                        u.id = dr.GetInt32(0);
                        u.rol_id = dr.GetInt32(1);
                        r.id= dr.GetInt32(2);
                        r.rol=dr.GetString(3);
                        u.roles= r;
                        u.nombre= dr.GetString(4);
                        u.apellido= dr.GetString(5);
                        u.sexo_id= dr.GetInt32(6);
                        s.id = dr.GetInt32(7);
                        s.sexo1 = dr.GetString(8);
                        u.sexo = s;
                        u.telefono = dr.GetString(9);
                        u.email= dr.GetString(10);
                        u.estudiante =dr.GetBoolean(11);
                        
                        user.Add(u);
                    }
                }
            }

            List<estudiante> estudiante= new List<estudiante>();
            string sqlEstudiante = "SELECT * FROM estudiante WHERE usuario_id = " + id + ";";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmdEstudiante = new SqlCommand(sqlEstudiante, cn);
                cn.Open();
                using (SqlDataReader drEstudiante = cmdEstudiante.ExecuteReader())
                {
                    while (drEstudiante.Read())
                    {
                        estudiante e = new estudiante();
                        e.id = drEstudiante.GetInt32(0);
                        e.matricula = drEstudiante.GetString(1);
                        e.carrera = drEstudiante.GetString(2);
                        e.nivelEstudios = drEstudiante.GetString(3);
                        e.usuario_id = drEstudiante.GetInt32(4);
                        estudiante.Add(e);
                    }
                }
            }

            ViewModelUsuarioEstudiante viewModel = new ViewModelUsuarioEstudiante()
            {
                user = user,
                student = estudiante
            };

            if (viewModel == null)
            {
                return View("NULL");
            }
            else if (viewModel.user == null || viewModel.student == null)
            {
                return View("EmptyView");
            }

            if (id >= 0)
            {
                if (ModelState.IsValid)
                {
                    return View(viewModel);
                }
                else
                {

                    return View();
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
        }

        [HttpPost]
        public ActionResult EditaUsuario(usuario umodel)
        {
            try
            {
                usuario udb = new usuario();
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
                usuario u = new usuario();
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

            List<curso> listCursos = new List<curso>();
            usuario u = (usuario)HttpContext.Session["usuario"];
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_obtenCursosUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("IdUsuario", u.id);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        curso c = new curso();
                        modalidad m = new modalidad();
                        
                        listCursos.Add(c);
                    }
                }

            }



            ViewModelCursoUsuario vmcu = new ViewModelCursoUsuario();
            dynamic dynModel = new ExpandoObject();
            dynModel.cursos = vmcu.GetMisCursos();
            return View(dynModel);
        }

    }
}