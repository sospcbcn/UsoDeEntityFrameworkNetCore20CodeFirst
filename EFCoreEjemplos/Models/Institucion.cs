using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreEjemplos.Models
{
    public class Institucion
    {
        public Institucion()
        {
            Estudiantes = new List<Estudiante>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Estudiante> Estudiantes { get; set; }
    }
}
