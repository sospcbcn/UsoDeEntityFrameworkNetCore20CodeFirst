using EFCoreEjemplos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreEjemplos.Controlers
{
    public interface IEstudianteCursoControler
    {
        List<EstudianteCurso> ObtenerListadoAsignacionesCurso(int idCurso);
    }
}
