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
            //ViewModelUsuarioEstudiante vmue = new ViewModelUsuarioEstudiante();
            usuario user = new usuario();
            roles r = new roles();
            sexo s = new sexo();
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
                        user.id = dr.GetInt32(0);
                        user.rol_id= dr.GetInt32(1);
                        r.id = dr.GetInt32(2);
                        r.rol = dr.GetString(3);
                        user.roles = r;
                        user.nombre= dr.GetString(4);
                        user.apellido = dr.GetString(5);
                        user.sexo_id= dr.GetInt32(6);
                        s.id = dr.GetInt32(7);
                        s.sexo1 = dr.GetString(8);
                        user.sexo = s;
                        user.telefono = dr.GetString(9);
                        user.email = dr.GetString(10);
                        user.estudiante = dr.GetBoolean(11);
                    }
                }
            }


           /* List<estudiante> estudiante= new List<estudiante>();*/
            estudiante est = new estudiante();
            string sqlEstudiante = "SELECT * FROM estudiante WHERE usuario_id = " + id + ";";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmdEstudiante = new SqlCommand(sqlEstudiante, cn);
                cn.Open();
                using (SqlDataReader drEstudiante = cmdEstudiante.ExecuteReader())
                {
                    while (drEstudiante.Read())
                    {

                        est.id= drEstudiante.GetInt32(0);
                        est.matricula= drEstudiante.GetString(1);
                        est.carrera= drEstudiante.GetString(2);
                        est.nivelEstudios = drEstudiante.GetString(3);
                        est.usuario_id = drEstudiante.GetInt32(4); 
                    }
                }
            }
            ViewBag.usuario = user;
            ViewBag.estudiante = est;
            if (id >= 0)
            {
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
        }



        [HttpPost]
        public ActionResult EditaUsuario(usuario user, estudiante est)
        {
            ViewBag.usuario = user;
            ViewBag.estudiante = est;
            try
            {
                bool editado;
                string mensaje;
                int i;
                if (user.pass == user.passConfirm)
                {
                    user.pass = EncriptarSha256(user.pass);
                }
                else
                {
                    return View();
                }
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_editaUsuario", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Id", user.id);
                    cmd.Parameters.AddWithValue("Rol", user.rol_id);
                    cmd.Parameters.AddWithValue("Nombre", user.nombre);
                    cmd.Parameters.AddWithValue("Apellido", user.apellido);
                    cmd.Parameters.AddWithValue("Sexo", user.sexo_id);
                    cmd.Parameters.AddWithValue("Telefono", user.telefono);
                    cmd.Parameters.AddWithValue("Email", user.email);
                    cmd.Parameters.AddWithValue("Pass", user.pass);
                    cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cn.Open();
                    i = cmd.ExecuteNonQuery();
                    editado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    cn.Close();
                }

                if (user.estudiante == true)
                {
                    using (SqlConnection cn = new SqlConnection(cadenaConexion))
                    {
                        SqlCommand cmd = new SqlCommand("", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        //...

                        cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        cn.Open();
                        i = cmd.ExecuteNonQuery();
                        editado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                        mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                        cn.Close();
                    }
                }

                if (i >= 1)
                {
                    return RedirectToAction("TablasUsuarios", "Admin");
                }
                else
                {
                    return View();
                }               
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
        [ValidarSesion]
        public ActionResult CursosUsuario()
        {

            List<ViewModelUsuarioCurso> listCursos = new List<ViewModelUsuarioCurso>();
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
                        ViewModelUsuarioCurso usuarioCurso= new ViewModelUsuarioCurso();

                        usuarioCurso.id = dr.GetInt32(0);
                        usuarioCurso.idCurso = dr.GetInt32(1);
                        usuarioCurso.nombreCurso = dr.GetString(2);
                        usuarioCurso.idModalidad = dr.GetInt32(3);
                        usuarioCurso.modalidad1 = dr.GetString(5);
                        usuarioCurso.lugar = dr.GetString(6);
                        usuarioCurso.horas = dr.GetInt32(7);
                        usuarioCurso.fechaIni = dr.GetDateTime(8);
                        usuarioCurso.fechaTer = dr.GetDateTime(9);
                        usuarioCurso.costo = dr.GetDecimal(10);
                        usuarioCurso.costoPref = dr.GetDecimal(11);
                        usuarioCurso.urlTemario = dr.GetString(12);
                        usuarioCurso.requisitos = dr.GetString(13);
                        usuarioCurso.criterioEval = dr.GetString(14);
                        usuarioCurso.imgUrl = dr.IsDBNull(15) ? null : dr.GetString(15);
                        usuarioCurso.idEstatus = dr.GetInt32(16);

                        listCursos.Add(usuarioCurso);
                    }
                }

            }

            ViewBag.cursos = listCursos;

            
            return View();
        }

    }
}