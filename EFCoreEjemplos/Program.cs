using EFCoreEjemplos.Controlers;
using EFCoreEjemplos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EFCoreEjemplos
{
    class Program
    {
        private static EstudiantesControler ControlEstudiantes = new EstudiantesControler();
        private static InstitucionesControler ControlInstituciones = new InstitucionesControler();
        private static EstudianteCursoControler ControlEstudianteCurso = new EstudianteCursoControler();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //int numeroAlumnos =ControlEstudiantes.ObtenerCantidadEstudiantesPorIdInstitucionPorEdad(1, 4000);

            //ControlInstituciones.ObtenerEstudiantesPorIdInstitucionAndEdad(1, 30);

            //List<EstudianteCurso> asignacionesCurso = ControlEstudianteCurso.AsignarCursoParaEstudiante(1002);

            List<Institucion> listaSQLInstituciones = ControlInstituciones.ObtenerInstitucionesPorSql("Select * From dbo.Instituciones Where Nombre='Pepe'");

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Database.Migrate();
                if (!context.Instituciones.Any())
                {
                    // Sino hay Datos Rellenar la Base de Datos con datos pregenerados.
                    SeedDatabase();
                }
            }

            List<Estudiante> EstudiantesList = ControlEstudiantes.ObtenerListaEstudiantesConTodoDato();
            foreach(Estudiante estudiante in EstudiantesList)
            {
                Console.WriteLine(estudiante);
            }
            Console.WriteLine("Listo");
            Console.ReadLine();
        }

        // Usar este método para llenar la base de datos con data de prueba
        static void SeedDatabase()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Institucion institucion1 = new Institucion() { Nombre= "Institucion 1" };
                Institucion institucion2 = new Institucion() { Nombre = "Institucion 2" };


                Estudiante estudiante1 = ControlEstudiantes.GenerarEstudiante("Felipe", 999,true, "Ingeniería de Software", 1,"Av. Direccion estudiante 1");
                Estudiante estudiante2 = ControlEstudiantes.GenerarEstudiante("Claudia", 15,false, "Ingeniería de Software",1, "Calle Direccion estudiante 2");
                Estudiante estudiante3 = ControlEstudiantes.GenerarEstudiante("Roberto", 25,true, "Licenciatura en Derecho", 2, "Plaza Direccion estudiante 3");

                Curso curso1 = new Curso() { Nombre = "Calculo" };
                Curso curso2 = new Curso() { Nombre = "Algebra Lineal" };

                institucion1.Estudiantes.Add(estudiante1);
                institucion1.Estudiantes.Add(estudiante2);

                institucion2.Estudiantes.Add(estudiante3);

                context.Add(institucion1);
                context.Add(institucion2);
                context.Add(curso1);
                context.Add(curso2);

                context.SaveChanges();

                var estudianteCurso = new EstudianteCurso();
                estudianteCurso.Activo = true;
                estudianteCurso.CursoId = curso1.Id;
                estudianteCurso.EstudianteId = estudiante1.Id;

                var estudianteCurso2 = new EstudianteCurso();
                estudianteCurso2.Activo = false;
                estudianteCurso2.CursoId = curso1.Id;
                estudianteCurso2.EstudianteId = estudiante2.Id;

                context.Add(estudianteCurso);
                context.Add(estudianteCurso2);
                context.SaveChanges();
            }
        }


        static void AgregarModeloUnoAUnoConectado()
        {
            using (var context = new ApplicationDbContext())
            {
                // Aquí agregamos un nuevo estudiante y su dirección
                var estudiante = new Estudiante();
                estudiante.Nombre = "Claudio";
                estudiante.Edad = 99;

                var direccion = new Direccion();
                direccion.Calle = "Ejemplo";
                estudiante.Direccion = direccion;

                context.Add(estudiante);
                context.SaveChanges();
            }
        }

        static void AgregarModeloUnoAUnoModeloDesconectado(Direccion direccion)
        {
            // Modelo desconectado (el campo direccion.EstudianteId debe estar lleno)
            using (var context = new ApplicationDbContext())
            {
                context.Entry(direccion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }

        }

        static void TraerDataRelacionada()
        {
            // Utilizamos include para indicar que queremos traer los estudiantes y sus direcciones
            using (var context = new ApplicationDbContext())
            {
                var estudiantes = context.Estudiantes.Include(x => x.Direccion).ToList();
            }
        }

        static void AgregarModeloMuchosAMuchosModeloDesconectado(Estudiante estudiante)
        {
            // el campo estudiante.InstitucionId debe estar lleno
            using (var context = new ApplicationDbContext())
            {
                context.Add(estudiante);
                context.SaveChanges();
            }
        }

        static void TraerDataRelacionadaUnoAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {

                var institucionesEstudiantes1 = context.Instituciones.Where(x => x.Id == 1).Include(x => x.Estudiantes).ToList();

                // error 
                // var institucion = context.Instituciones.Where(x => x.Id == 1).Include(x => x.Estudiantes.Where(e => e.Edad > 18)).ToList();

                // proyección
                //var persona = context.Estudiantes.Select(x => new { prop =  x.Id, prop1 = x.Nombre }).FirstOrDefault();    

                var institucionesEstudiantes = context.Instituciones.Where(x => x.Id == 1)
                    .Select(x => new { Institucion = x, Estudiantes = x.Estudiantes.Where(e => e.Edad > 18).ToList() }).ToList();

            }
        }

        static void InsertarDataRelacionadaMuchosAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {
                var estudiante = context.Estudiantes.FirstOrDefault();
                var curso = context.Cursos.FirstOrDefault();

                var estudianteCurso = new EstudianteCurso();

                estudianteCurso.CursoId = curso.Id;
                estudianteCurso.EstudianteId = estudiante.Id;
                estudianteCurso.Activo = true;

                context.Add(estudianteCurso);
                context.SaveChanges();
            }
        }

        static void TraerDataRelacionadaMuchosAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {
                var curso = context.Cursos.Where(x => x.Id == 1).Include(x => x.EstudiantesCursos)
                    .ThenInclude(y => y.Estudiante).FirstOrDefault();
            }
        }

        static void FiltroPorTipo()
        {
            using (var context = new ApplicationDbContext())
            {
                // El filtro definido en el ApplicationDbContext.cs se aplica automáticamente
                var estudiantesCursos = context.EstudiantesCursos.ToList();
            }
        }

        static void FuncionEscalarEnEF()
        {
            using (var context = new ApplicationDbContext())
            {
                var estudiantes = context.Estudiantes
                    .Where(x => ApplicationDbContext.Cantidad_De_Cursos_Activos(x.Id) > 0).ToList();
            }
        }

        static void FuncionalidadTableSplitting()
        {
            using (var context = new ApplicationDbContext())
            {
                // Para insertar un nuevo estudiante ahora necesitamos colocar la info del detalle
                //var estudiante = new Estudiante();
                //estudiante.Nombre = "Carlos";
                //estudiante.Edad = 45;
                //estudiante.EstaBorrado = false;
                //estudiante.InstitucionId = 1;

                //var detalle = new EstudianteDetalle();
                //detalle.Becado = false;
                //detalle.Carrera = "Lic. en Matemáticas";
                //detalle.CategoriaDePago = 1;

                //estudiante.Detalles = detalle;
                //context.Add(estudiante);
                //context.SaveChanges();

                var estudiantes = context.Estudiantes.Include(x => x.Detalles).ToList();
            }
        }
    }
}
