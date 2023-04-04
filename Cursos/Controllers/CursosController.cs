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
using Cursos.Permisos;
using Cursos.BDConnection;
using System.Security.Cryptography;
using System.Data.Entity.Infrastructure;
using System.Web.Services.Description;

namespace Cursos.Controllers
{

    public class CursosController : Controller
    {
        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;

        //*****************REGISTRA CURSO****************************

        [ValidarSesionAdmin]
        public ActionResult RegistrarCurso()
        {
            return View();
        }


        [HttpPost]
        public ActionResult RegistrarCurso(curso cmodel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    curso cdb = new curso();
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
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        fname = file.FileName;
                        fname = Path.Combine(Server.MapPath("~/Recursos/imgs"), fname);
                        file.SaveAs(fname);
                    }
                    return Json("Se cargo correctamente");
                }
                catch (Exception ex)
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
        [ValidarSesionAdmin]
        [HttpGet]
        public ActionResult EditaCurso(int id)
        {

            curso c = new curso();
            modalidad m = new modalidad();
            string sql = "select curso.id, curso.nombre, curso.idModalidad, modalidad.id, modalidad.modalidad, curso.lugar, curso.horas, " +
                "curso.fechaIni, curso.fechaTer, curso.costo, curso.costoPref, curso.urlTemario, curso.requisitos, curso.criterioEval, curso.imgUrl from curso " +
                "inner join modalidad on modalidad.id = curso.idModalidad;";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    { 
                        c.id = dr.GetInt32(0);
                        c.nombre = dr.GetString(1);
                        c.idModalidad = dr.GetInt32(2);
                        m.id = dr.GetInt32(3);
                        m.modalidad1 = dr.GetString(4);
                        c.modalidad = m;
                        c.lugar = dr.GetString(5);
                        c.horas = dr.GetInt32(6);
                        c.fechaIni = dr.GetDateTime(7);
                        c.fechaTer = dr.GetDateTime(8);
                        c.costo = dr.GetDecimal(9);
                        c.costoPref = dr.GetDecimal(10);
                        c.urlTemario = dr.GetString(11);
                        c.requisitos = dr.GetString(12);
                        c.criterioEval = dr.GetString(13);
                        c.imgUrl = dr.IsDBNull(14) ? null : dr.GetString(14);
                    }
                }
            }

            ViewBag.curso = c;

            return View();
        }

        [HttpPost]
        public ActionResult EditaCurso(curso cursosInfo)
        {
            ViewBag.curso = cursosInfo;
            try
            {
                bool editado;
                string mensaje;
                int i;
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_editaCurso", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Id", cursosInfo.id);
                    cmd.Parameters.AddWithValue("Nombre", cursosInfo.nombre);
                    cmd.Parameters.AddWithValue("Modalidad", cursosInfo.modalidad);
                    cmd.Parameters.AddWithValue("Lugar", cursosInfo.lugar);
                    cmd.Parameters.AddWithValue("Horas", cursosInfo.horas);
                    cmd.Parameters.AddWithValue("FechaIni", cursosInfo.fechaIni);
                    cmd.Parameters.AddWithValue("FechaTer", cursosInfo.fechaTer);
                    cmd.Parameters.AddWithValue("Costo", cursosInfo.costo);
                    cmd.Parameters.AddWithValue("CostoPref", cursosInfo.costoPref);
                    cmd.Parameters.AddWithValue("UrlTemario", cursosInfo.urlTemario);
                    cmd.Parameters.AddWithValue("Requisitos", cursosInfo.requisitos);
                    cmd.Parameters.AddWithValue("CriterioEval", cursosInfo.criterioEval);
                    cmd.Parameters.AddWithValue("ImgUrl", cursosInfo.imgUrl);
                    cmd.Parameters.Add("Editado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    cn.Open();
                    i = cmd.ExecuteNonQuery();

                    editado = Convert.ToBoolean(cmd.Parameters["Editado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    cn.Close();
                }

                if (i >= 1)
                {
                    return RedirectToAction("Tablas", "Admin");
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

        //*****************ELIMINA CURSO****************************
        [ValidarSesionAdmin]
        public ActionResult EliminaCurso(int id)
        {
            try
            {
                curso c = new curso();
                if (c.eliminarCurso(id))
                {
                    ViewBag.AlertMsg = "El cursos se ha eliminado";
                }
                return RedirectToAction("Tablas", "Admin");
            }
            catch
            {
                return RedirectToAction("Tablas", "Admin");
            }
        }

        //*****************REGISTRA USUARIO A CURSO****************************

        [ValidarSesion]
        [HttpPost]
        public ActionResult RegistraCursoUsuario(cursoUsuario cu, situacionFiscal sf)
        {
            bool registrado;
            string mensaje;
            int i;
            if (HttpContext.Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                //usuario u = (usuario)HttpContext.Session["usuario"];
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {

                    SqlCommand cmd = new SqlCommand("SP_registraCursoUsuario", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IdUsuario", cu.idUsuario);
                    cmd.Parameters.AddWithValue("IdCurso", cu.idCurso);
                    if (cu.factura == true)
                    {
                        cmd.Parameters.AddWithValue("Factura", 1);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("Factura", 0);
                    }

                    cmd.Parameters.AddWithValue("FechaVenc", cu.fechaVenc);
                    cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;


                    cn.Open();
                    i = cmd.ExecuteNonQuery();
                    registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    cn.Close();
                }

                if (cu.factura == true)
                {
                    InsertaFactura(cu, sf);
                }


                if (i >= 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("FichaDePago", "Cursos");
                }
            }

        }

        //*****************REGISTRA FACTURA****************************
        [ValidarSesion]
        public void InsertaFactura(cursoUsuario cu, situacionFiscal sf)
        {
            bool registrado;
            string mensaje;
            int i;
            string sql = "select id from cursoUsuario " +
                "where idUsuario = " + cu.idUsuario + " and idCurso =" + cu.idCurso + ";"; 
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        cu.id = dr.GetInt32(0);
                    }
                }
            }

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {

                SqlCommand cmd = new SqlCommand("SP_registraFactura", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("RFC", sf.rfc);
                if(sf.documento != null)
                {
                    cmd.Parameters.AddWithValue("Documento", sf.documento);
                }
                cmd.Parameters.AddWithValue("idCursoUsuario", cu.id);
                cmd.Parameters.AddWithValue("Email", sf.email);
                cmd.Parameters.AddWithValue("CP", sf.codigoPostal);
                cmd.Parameters.AddWithValue("Activado", sf.activado);

                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cn.Open();
                i = cmd.ExecuteNonQuery();
                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                cn.Close();
            }

        }

        //*****************FICHA DE PAGO****************************
        [ValidarSesion]
        public ActionResult FichaDePago(int id)
        {
            curso vc = new curso();
            usuario u = new usuario();
            dynamic dynModel = new ExpandoObject();
            dynModel.curso = vc.GetCursos().Find(cmodel => cmodel.id == id);
            dynModel.usuario = u.GetUsuario();
            return View(dynModel);
        }


        //*****************CARGA DOCUMENTOS****************************
        [ValidarSesion]
        [HttpPost]
        public ActionResult CargaDocumentos(int id, HttpPostedFileBase comprobantePago, HttpPostedFileBase comprobanteId)
        {
            usuario u = (usuario)HttpContext.Session["usuario"];
            string hash1 = "";
            string hash2 = "";
            int i;

            if (comprobanteId != null && comprobanteId.ContentLength > 0)
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = comprobanteId.InputStream)
                    {
                        hash1 = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                    }
                }

                var nombreArchivo = hash1 + ".pdf";
                var rutaArchivo = Path.Combine(Server.MapPath("~/Archivos/DocumentosUsuario"), nombreArchivo);
                comprobantePago.SaveAs(rutaArchivo);

            }

            if (comprobantePago != null && comprobantePago.ContentLength > 0)
            {
                using(var md5 = MD5.Create())
                {
                    using(var stream = comprobantePago.InputStream)
                    {
                        hash2 = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-","").ToLower();
                    }
                }

                var nombreArchivo = hash2 + ".pdf";
                var rutaArchivo = Path.Combine(Server.MapPath("~/Archivos/Comprobantes"), nombreArchivo);
                comprobantePago.SaveAs(rutaArchivo);
                
            }
            string sql = "UPDATE cursoUsuario SET comprobantePago = @hash1, dIdUsuario = @hash2  WHERE id = " + id;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@hash1", hash1);
                cmd.Parameters.AddWithValue("@hash2", hash2);
                cn.Open();
                i = cmd.ExecuteNonQuery();
                cn.Close();
            }

            return RedirectToAction("CursosUsuario", "Usuario"); ;

        }

        //******************OBTEN DATOS PARA ARCHVIO*******************
        [ValidarSesion]
        [HttpPost]
        public bool obtenDatos(ViewModelUsuarioCurso cu)
        {
            string curso;
            string usuario;
            int i;
          
            string sql = "SELECT curso.nombre, usuario.nombre FROM cursoUsuario " +
            "INNER JOIN curso ON curso.id = cursoUsuario.idCurso " +
            "INNER JOIN usuario ON usuario.id = cursoUsuario.idUsuario " +
            "WHERE cursoUsuario.id = " + cu.id;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    curso = dr.GetString(0);
                    usuario = dr.GetString(1);
                }
                i = cmd.ExecuteNonQuery();
                cn.Close();
            }
            if(i > 0){
                return true;
            }
            else
            {
                return false;
            }

        }


        //******************VER DETALLES DE CURSO*******************
        [ValidarSesionAdmin]
        [HttpGet]
        public ActionResult VerCursoDetalles(int id)
        {
            List<usuario> usuarios = new List<usuario>();
            List<estudiante> estudiantes = new List<estudiante>();
            List<curso> curso = new List<curso>();
            string sql = "SELECT curso.id, curso.nombre, curso.idModalidad, modalidad.id, modalidad.modalidad, " +
                "curso.lugar, curso.horas, curso.fechaIni, curso.fechaTer, curso.urlTemario from curso " +
                "INNER JOIN modalidad ON modalidad.id = curso.idModalidad " +
                "WHERE curso.id = " + id + ";";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        curso c = new curso();
                        modalidad m = new modalidad();
                        c.id = dr.GetInt32(0);
                        c.nombre = dr.GetString(1);
                        c.idModalidad = dr.GetInt32(2);
                        m.id = dr.GetInt32(3);
                        m.modalidad1 = dr.GetString(4);
                        c.modalidad = m;
                        c.lugar = dr.GetString(5);
                        c.horas = dr.GetInt32(6);
                        c.fechaIni = dr.GetDateTime(7);
                        c.fechaTer = dr.GetDateTime(8);
                        c.urlTemario = dr.GetString(9);
                        curso.Add(c);
                    }
                    cn.Close();
                }
            }

            sql = "SELECT usuario.id, usuario.nombre, usuario.apellido, usuario.sexo_id, " +
                "sexo.id, sexo.sexo, usuario.rol_id, roles.id, roles.rol, usuario.email, " +
                "usuario.telefono, usuario.estudiante, cursoUsuario.id " +
                "from usuario " +
                "inner join sexo on sexo.id = usuario.sexo_id " +
                "inner join roles on roles.id = usuario.rol_id " +
                "inner join cursoUsuario on cursoUsuario.idUsuario = usuario.id " +
                "where cursoUsuario.idCurso = " + id + ";";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usuario u = new usuario();
                        sexo s = new sexo();
                        roles r = new roles();
                        estudiante e = new estudiante();
                        u.id = dr.GetInt32(0);
                        u.nombre = dr.GetString(1);
                        u.apellido = dr.GetString(2);
                        u.sexo_id = dr.GetInt32(3);
                        s.id = dr.GetInt32(4);
                        s.sexo1 = dr.GetString(5);
                        u.sexo = s;
                        u.rol_id = dr.GetInt32(6);
                        r.id = dr.GetInt32(7);
                        r.rol = dr.GetString(8);
                        u.email = dr.GetString(9);
                        u.telefono = dr.GetString(10);
                        u.estudiante = dr.GetBoolean(11);
                        if (u.estudiante == true)
                        {

                            sql = "SELECT estudiante.id, estudiante.matricula, estudiante.carrera, " +
                                "estudiante.nivelEstudios, estudiante.usuario_id, usuario.id from estudiante " +
                                "inner join usuario on usuario.id = estudiante.usuario_id;";
                            using (SqlConnection con = new SqlConnection(cadenaConexion))
                            {
                                con.Open();
                                SqlCommand sqlcmd = new SqlCommand(sql, con);
                                using (SqlDataReader sqldr = sqlcmd.ExecuteReader())
                                {
                                    while (sqldr.Read())
                                    {


                                        e.id = sqldr.GetInt32(0);
                                        e.matricula = sqldr.GetString(1);
                                        e.carrera = sqldr.GetString(2);
                                        e.nivelEstudios = sqldr.GetString(3);
                                        e.usuario_id = sqldr.GetInt32(4);
                                        u.id = sqldr.GetInt32(5);
                                        e.usuario = u;
                                        estudiantes.Add(e);
                                    }
                                    con.Close();
                                }
                            }
                        }

                        usuarios.Add(u);

                    }
                    cn.Close();
                }
            }
            ModelViewCursoUsuarios viewModel = new ModelViewCursoUsuarios()
            {
                course = curso,
                student = estudiantes,
                user = usuarios
            };

            return View(viewModel);
        }

        //******************VER USUARIOS INSCRITOS*******************
        [ValidarSesionAdmin]
        [HttpGet]
        public ActionResult usuariosInscritos()
        {
            List<ViewModelUsuarioCurso> list = new List<ViewModelUsuarioCurso>();  
            /*string sql = "SELECT usuario.id, usuario.nombre,  usuario.apellido, " +
                "usuario.telefono,  STRING_AGG(curso.nombre, ','), cursoUsuario.idUsuario" +
                "FROM cursoUsuario " +
                "JOIN usuario ON usuario.id = cursoUsuario.idUsuario " +
                "JOIN curso ON cursoUsuario.idCurso = curso.id " +
                "WHERE cursoUsuario.idUsuario = usuario.id " +
                "GROUP BY usuario.id, usuario.nombre, usuario.apellido, usuario.telefono, cursoUsuario.idUsuario;";
            */
            string sql = "select * from inscritosGlobal_view";

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ViewModelUsuarioCurso vmuc = new ViewModelUsuarioCurso();


                        vmuc.idUsuario = dr.GetInt32(0);
                        vmuc.nombreUsuario = dr.GetString(1);
                        vmuc.apellido = dr.GetString(2);
                        vmuc.telefono = dr.GetString(3);
                        vmuc.nombreCurso = string.Join(", ", dr.GetString(4).Split(','));
                        
                        list.Add(vmuc);
                    }
                cn.Close();                    
                }
            }
            
            ViewBag.UsuarioCurso = list;

            return View();
        }
    }

}
