using EFCoreEjemplos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFCoreEjemplos.Controlers
{
    public class EstudianteCursoControler : IEstudianteCursoControler
    {
        public List<EstudianteCurso> ObtenerListadoAsignacionesCurso(int idCurso)
        {
            List<EstudianteCurso> result = new List<EstudianteCurso>();

            try
            { 
            using (var context = new ApplicationDbContext())
            {
                Curso cursos = context.Cursos.Where(x => x.Id == idCurso).Include(x => x.EstudiantesCursos).ThenInclude(y => y.Estudiante).FirstOrDefault();
                result=cursos.EstudiantesCursos.ToList();
            }
            }

            catch
            {
            }

            return result;
        }
    }
}
