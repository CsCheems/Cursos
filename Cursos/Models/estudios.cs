namespace Cursos.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("estudios")]
    public  partial class estudios
    {
        private HashSet<usuarios> usuarios;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public estudios()
        {
            usuarios = new HashSet<usuarios>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Column("estudios")]
        [StringLength(50)]
        public string estudiosNombre { get; set; }

    }
}