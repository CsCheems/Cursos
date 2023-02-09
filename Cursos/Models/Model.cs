using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Cursos.Models
{
    public partial class Model : DbContext
    {
        public IEnumerable<cursos> cursos { get; set; }
        public IEnumerable<modalidad> modalidad { get; set; }
        public IEnumerable<rol> rol { get; set; }
        public IEnumerable<usuarios> usuarios { get; set; }

        

        public Model()
            : base("name=Model")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<cursos>()
                .Property(e => e.modalidad)
                .IsUnicode(false);

            modelBuilder.Entity<cursos>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<cursos>()
                .Property(e => e.lugar)
                .IsUnicode(false);

            modelBuilder.Entity<cursos>()
                .Property(e => e.costo)
                .HasPrecision(8, 2);

            modelBuilder.Entity<cursos>()
                .Property(e => e.costoPref)
                .HasPrecision(8, 2);

            modelBuilder.Entity<cursos>()
                .Property(e => e.urlTemario)
                .IsUnicode(false);

            modelBuilder.Entity<cursos>()
                .Property(e => e.requisitos)
                .IsUnicode(false);

            modelBuilder.Entity<cursos>()
                .Property(e => e.criterioEval)
                .IsUnicode(false);

            modelBuilder.Entity<cursos>()
                .Property(e => e.imgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<usuarios>()
                .Property(e => e.rol)
                .IsUnicode(false);

            modelBuilder.Entity<usuarios>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<usuarios>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<usuarios>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<usuarios>()
                .Property(e => e.pass)
                .IsUnicode(false);
        }

    }
}
