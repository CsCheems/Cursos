namespace Cursos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class usuarios
    {
        public int id { get; set; }

        public int rol { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(50)]
        public string apellido { get; set; }

        [StringLength(255)]
        public string email { get; set; }

        [StringLength(20)]
        public string pass { get; set; }

        [StringLength(20)]
        public string passConfirm { get; set; }

        public virtual rol rol1 { get; set; }


        //Metodos
        

    }
}
