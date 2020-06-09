using EFCoreEjemplos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreEjemplos.Controlers
{
    public interface IEstudiantesControler
    {
        List<Estudiante> ObtenerListaEstudiantesConTodoDato();
        Estudiante GenerarEstudiante(string nombre, int edad, bool becado, string carrera, int categoriaDePago, string direccion);
        int ObtenerCantidadEstudiantesPorIdInstitucionPorEdad(int id, int edadCorte);
        void ActualizarEstudianteModeloConectado(int id);
        void ActualizarEstudianteModeloDesconectado(Estudiante estudiante);
        void BorradoFisicoEstudianteModeloConectado(int id);
        void BorradoFisicoEstudianteModeloDesonectado(Estudiante estudiante);
    }
}
