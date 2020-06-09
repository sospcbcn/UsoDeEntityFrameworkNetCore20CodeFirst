using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFCoreEjemplos.Models
{
    public class Curso
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Nombre { get; set; }
        public List<EstudianteCurso> EstudiantesCursos { get; set; }
    }
}
