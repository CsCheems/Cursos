namespace Cursos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class cursos
    {
        public int id { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        public int modalidad { get; set; }

        [StringLength(255)]
        public string lugar { get; set; }

        public int? horas { get; set; }

        public decimal? costo { get; set; }

        public decimal? costoPref { get; set; }

        [StringLength(255)]
        public string urlTemario { get; set; }

        [StringLength(100)]
        public string requisitos { get; set; }

        [StringLength(100)]
        public string criterioEval { get; set; }

        [StringLength(255)]
        public string imgUrl { get; set; }

        public virtual modalidad modalidad1 { get; set; }

        //Metodos
        
    }
}
