using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreEjemplos.Models
{
    public class Direccion
    {
        public int Id { get; set; }
        public string Calle { get; set; }
        public int EstudianteId { get; set; }
    }
}
