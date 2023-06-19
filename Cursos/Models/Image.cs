using Cursos.BDConnection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity.Spatial;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Services.Description;
namespace Cursos.Models
{
    public class Image
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public byte[] imagen { get; set; }

        static string cadenaConexion = SQL_DB_Connection.cadenaConexion;

        public List<Image> GetImages()
        {
            List<Image> listImage = new List<Image>();
            string sql = "SELECT nombre, imagen FROM imagenesFront";
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Image i = new Image();
                        i.nombre = dr["nombre"].ToString();
                        i.imagen = (byte[])dr["imagen"];
                        listImage.Add(i);
                    }
                }
            }
            return listImage;
        }
    }
}

   