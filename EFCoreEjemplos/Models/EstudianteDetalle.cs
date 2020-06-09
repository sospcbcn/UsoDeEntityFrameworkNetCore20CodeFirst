using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreEjemplos.Models
{
    public class EstudianteDetalle
    {
        public int Id { get; set; }
        public bool Becado { get; set; }
        public string Carrera { get; set; }
        public int CategoriaDePago { get; set; }
        public Estudiante Estudiante { get; set; }
    }
}
