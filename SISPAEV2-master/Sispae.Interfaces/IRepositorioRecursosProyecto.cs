using Sispae.Entities.MRecursosProyecto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioRecursosProyecto
    {
        Task<List<VRecursosProyecto>> GetRecursosProyecto(int integracion);
        Task<int> insertaRecursosProyecto(RecursosProyectos unidad);
        Task<int> eliminaRecursoProyecto(int integracion, int mes, decimal monto);
    }
}
