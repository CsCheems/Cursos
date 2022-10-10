namespace Cursos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;


    [Table("estatus")]
    public partial class estatus
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(20)]
        public string estatus1 { get; set; }

        public List<estatus> Listar()
        {
            var estado = new List<estatus>();
            string query = "SELECT * FROM estatus";
            try
            {
                using(var container = new Model())
                {
                    estado = container.Database.SqlQuery<estatus>(query).ToList();
                }
            }
            catch (Exception)
            {
                //throw
            }
            return estado;
        }

    }
}