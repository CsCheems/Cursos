using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Cursos.Models
{
    public partial class Model : DbContext
    {
        public IEnumerable<curso> cursos { get; set; }
        public IEnumerable<modalidad> modalidad { get; set; }
        public IEnumerable<roles> rol { get; set; }
        public IEnumerable<usuario> usuarios { get; set; }

        

        public Model()
            : base("name=Model")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<curso>()
                .Property(e => e.nombre)
                .IsUnicode(false);


            modelBuilder.Entity<curso>()
                .Property(e => e.idModalidad);


            modelBuilder.Entity<curso>()
                .Property(e => e.lugar)
                .IsUnicode(false);

            modelBuilder.Entity<curso>()
                .Property(e => e.costo)
                .HasPrecision(8, 2);

            modelBuilder.Entity<curso>()
                .Property(e => e.costoPref)
                .HasPrecision(8, 2);

            modelBuilder.Entity<curso>()
                .Property(e => e.urlTemario)
                .IsUnicode(false);

            modelBuilder.Entity<curso>()
                .Property(e => e.requisitos)
                .IsUnicode(false);

            modelBuilder.Entity<curso>()
                .Property(e => e.criterioEval)
                .IsUnicode(false);

            modelBuilder.Entity<curso>()
                .Property(e => e.imgUrl);

            modelBuilder.Entity<usuario>()
                .Property(e => e.rol_id);

            modelBuilder.Entity<usuario>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<usuario>()
                .Property(e => e.pass)
                .IsUnicode(false);
        }

    }
}
