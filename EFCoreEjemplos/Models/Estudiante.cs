using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFCoreEjemplos.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        // Inyectando esta propiedad al campo Nombre, en el caso que se quiera modificar un Nombre y entre esa nueva asignación y el SaveChanges en otra parte se modifique justo ese nombre...
        // Saltará una excepción de concurrencia gracias al [ConcurrencyCheck].
        [ConcurrencyCheck]
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int InstitucionId { get; set; }
        public bool EstaBorrado { get; set; }

        private string _Apellido;

        public string Apellido
        {
            get { return _Apellido; }
            set
            {
                _Apellido = value.ToUpper();
            }
        }
        public Direccion Direccion { get; set; }
        public List<EstudianteCurso> EstudiantesCursos { get; set; }
        public EstudianteDetalle Detalles { get; set; }
    }
}
