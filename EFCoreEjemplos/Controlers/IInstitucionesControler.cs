using EFCoreEjemplos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreEjemplos.Controlers
{
    public interface IInstitucionesControler
    {
        Institucion ObtenerEstudiantesPorIdInstitucion(int id);
        void ObtenerEstudiantesPorIdInstitucionAndEdad(int id, int edadCorte);
        List<Institucion> ObtenerInstitucionesPorSql(string nombre);
        List<Institucion> ObtenerInstitucionesPorSqlConStringInterpolation(string nombre);
    }
}
