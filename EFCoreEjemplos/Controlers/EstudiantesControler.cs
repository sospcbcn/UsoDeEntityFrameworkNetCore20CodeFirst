using EFCoreEjemplos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EFCoreEjemplos.Controlers
{
    public class EstudiantesControler : IEstudiantesControler
    {

        public List<Estudiante> ObtenerListaEstudiantesConTodoDato()
        {
            List<Estudiante> result = new List<Estudiante>();

            try
            { 
                using (var context = new ApplicationDbContext())
                {
                    result = context.Estudiantes.Include(x => x.Direccion).Include(y => y.Detalles).ToList();
                }
            }

            catch
            {

            }

            return result;
        }

        public int ObtenerCantidadEstudiantesPorIdInstitucionPorEdad(int institucionId, int edadCorte)
        {
            int result = 0;

            try
            {
                using (var context = new ApplicationDbContext())
                {
                    result = context.Estudiantes
                        .Select(x => new { x.InstitucionId, x.Edad })
                        .Where(x => x.InstitucionId == institucionId && x.Edad < edadCorte)
                        .Count();
                }

            }

            catch
            {
            }

            return result;
        }

        public Estudiante GenerarEstudiante(string nombre, int edad, bool becado, string carrera, int categoriaDePago, string direccion)
        {
            Estudiante result = new Estudiante();

            try
            { 
            result.Nombre = nombre;
            result.Edad = edad;
            result.Detalles = new EstudianteDetalle() { Becado = becado, Carrera = carrera, CategoriaDePago = categoriaDePago };
            result.Direccion = new Direccion() { Calle = direccion };
            }

            catch
            {

            }

            return result;
        }

        public void ActualizarEstudianteModeloConectado(int id)
        {
            // El Modelo Conectado se utiliza en Aplicaciones de Escritorio
            try
            { 
                using (var context = new ApplicationDbContext())
                {
                    Estudiante estudiante = context.Estudiantes.Where(x => x.Id == id).FirstOrDefault();
                    estudiante.Nombre += " Apellido";
                    context.SaveChanges();
                }
            }

            catch
            {
            }
        }

        public void ActualizarEstudianteModeloDesconectado(Estudiante estudiante)
        {
            // En las Peticiones HTTP de Proyectos Web el Contexto de entrada de datos y el de modificar son distintos. En estos casos se aplica un modelo Desconectado.
            try
            { 
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(estudiante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            
            catch
            { 
            }
        }

        public void BorradoFisicoEstudianteModeloConectado(int id)
        {
            // El Modelo Conectado se utiliza en Aplicaciones de Escritorio
            // Para que realmente borre físicamente hay que quitar el override del SaveChanges en ApplicationDbContext
            try
            { 
                using (var context = new ApplicationDbContext())
                {
                    Estudiante estudiante = context.Estudiantes.Where(x => x.Id==id).FirstOrDefault();
                    if(estudiante != null)
                    { 
                        context.Remove(context);
                        context.SaveChanges();
                    }
                }
            }
            
            catch
            {
            }
        }

        public void BorradoFisicoEstudianteModeloDesonectado(Estudiante estudiante)
        {
            // En las Peticiones HTTP de Proyectos Web el Contexto de entrada de datos y el de borrado son distintos. En estos casos se aplica un modelo Desconectado.
            // Para que realmente borre físicamente hay que quitar el override del SaveChanges en ApplicationDbContext
            try
            { 
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(estudiante).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    context.SaveChanges();
                }
            }

            catch
            {
            }
        }

        public void BorradoLogicoEstudiante(int id)
        {
            try
            { 
            // El estudiante no será borrado (Ver el Override de SaveChanges en ApplicationDbContext)
                using (var context = new ApplicationDbContext())
                {
                    var estudiante = context.Estudiantes.FirstOrDefault();
                    context.Remove(estudiante);
                    context.SaveChanges();
                }
            }

            catch
            {
            }
        }
    }
}
