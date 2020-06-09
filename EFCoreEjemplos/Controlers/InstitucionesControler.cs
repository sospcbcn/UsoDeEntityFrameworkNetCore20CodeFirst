using EFCoreEjemplos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFCoreEjemplos.Controlers
{
    public class InstitucionesControler : IInstitucionesControler
    {
        public Institucion ObtenerEstudiantesPorIdInstitucion(int id)
        {
            Institucion result = new Institucion();
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    result = context.Instituciones.Where(x => x.Id == id).Include(y => y.Estudiantes).FirstOrDefault();
                }

            }

            catch
            {
            }

            return result;
        }

        public void ObtenerEstudiantesPorIdInstitucionAndEdad(int id, int edadCorte)
        {
            try
            { 
                using (var context = new ApplicationDbContext())
                {
                    var result = context.Instituciones
                        .Where(y => y.Id == id)
                        .Select(x => new { Institucion = x, Estudiantes = x.Estudiantes.Where(e => e.Edad < edadCorte).ToList()});

                }
            }

            catch
            {
            }
        }

        public List<Institucion> ObtenerInstitucionesPorSql(string nombre)
        {
            // La desventaja de usar este método tal y como está codificado es que es vulnerable a SQL Injections externos, nos pueden pasar una SQL como parámetro distinta a la esperada.
            // P.E. Where Nombre='Pepe' or 1=1 Si se inyecta el or 1=1 todos los registros cumplirán la condición y serán devueltos.

            List<Institucion> result = new List<Institucion>();

            try
            { 
                using (var context = new ApplicationDbContext())
                {
                    result=context.Instituciones.FromSqlRaw("Select * From dbo.Instituciones Where Nombre='" + nombre + "'").ToList();
                }
            }

            catch
            {
            }

            return result;
        }

        public List<Institucion> ObtenerInstitucionesPorSqlConStringInterpolation(string nombre)
        {
            // Es importante el uso de String Interpolation para evitar SQL Injections exteriores que podrían listar todos los datos.
            // Si como nombre pasamos 'Pepe' or 1=1 ahora todo lo más que puede suceder es que salte una excepción, y controlando las excepciones al saltar una se devuelve una lista vacía.
            List<Institucion> result = new List<Institucion>();

            try
            { 
                using (var context = new ApplicationDbContext())
                {
                    result=context.Instituciones.FromSqlRaw($"Select * From dbo.Instituciones Where Nombre='{nombre}'").ToList();
                }
            }

            catch
            {
            }

            return result;
        }
    }
}
