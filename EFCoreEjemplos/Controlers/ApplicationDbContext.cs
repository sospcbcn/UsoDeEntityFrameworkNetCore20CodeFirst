using EFCoreEjemplos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFCoreEjemplos.Controlers
{
    class ApplicationDbContext: DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // El connectionString debe venir de un archivo de configuraciones!
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=PruebaEfCoreConsola;Integrated Security=True")
                .EnableSensitiveDataLogging(true);
                //.UseLoggerFactory(new LoggerFactory().AddConsole((category, level) => level == LogLevel.Information && category == DbLoggerCategory.Database.Command.Name, true));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Agregamos una llave compuesta para tabla EstudiantesCursos
            modelBuilder.Entity<EstudianteCurso>().HasKey(x => new { x.CursoId, x.EstudianteId });

            // Filtro por tipo
            // Cualquier consulta que se haga sobre el Modelo indicado siempre aplicará el filtro Activo=True
            modelBuilder.Entity<EstudianteCurso>().HasQueryFilter(x => x.Activo == true);

            // Table splitting
            modelBuilder.Entity<Estudiante>().HasOne(x => x.Detalles)
                .WithOne(x => x.Estudiante)
                .HasForeignKey<EstudianteDetalle>(x => x.Id);
            modelBuilder.Entity<EstudianteDetalle>().ToTable("Estudiantes");

            // Mapeo Flexible
            modelBuilder.Entity<Estudiante>().Property(x => x.Apellido).HasField("_Apellido");
        }

        public override int SaveChanges()
        {
            // Borrado Lógico. Se recorren todas las entradas pendientes de modificar, cuyo EntityState sea Deleted(Dispuesto a ser borrado) y la Tabla tenga un campo llamado EstaBorrado.
            // Si se cumplen las condiciones el Campo EstaBorrado se pondrá a true en lugar de borrar físicamente el registro.
            // Por lo que una vez implantada esta sobreescritura si queremos que en una tabla no se eliminen físicamente los registros le tenemos que añadir el campo Bool EstaBorrado.
            foreach (var item in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted &&
               e.Metadata.GetProperties().Any(x => x.Name == "EstaBorrado")))
            {
                item.State = EntityState.Unchanged;
                item.CurrentValues["EstaBorrado"] = true;
            }

            return base.SaveChanges();
        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Institucion> Instituciones { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<EstudianteCurso> EstudiantesCursos { get; set; }

        [DbFunction(Schema = "dbo")]
        public static int Cantidad_De_Cursos_Activos(int EstudianteId)
        {
            throw new Exception();
        }

    }
}
