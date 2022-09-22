namespace Cursos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("rol")]
    public partial class rol
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rol()
        {
            usuarios = new HashSet<usuarios>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(20)]
        public string rolUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<usuarios> usuarios { get; set; }

        //Metodos
        public List<rol> Listar()
        {
            var roles = new List<rol>();
            string query = "SELECT * FROM rol";
            try
            {
                using (var container = new Model())
                {
                    roles = container.Database.SqlQuery<rol>(query).ToList();
                }
            }
            catch (Exception)
            {
                //throw;
            }

            return roles;

        }
    }
}
