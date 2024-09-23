using Sispae.Entities.MRecursos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioRecursos
    {
        Task<List<Recursos>> getRecursosBySeguimiento(int seguimiento);
        Task<Recursos> getRecursoById(int recurso);
        Task<int> insertaRecurso(Recursos recursos);
        Task<int> actualizaRecurso(Recursos recursos);
        Task<int> eliminaRecursos(Recursos recursos);
        Task<int> eliminaRecurso(Recursos recursos);
        Task<int> RecursosSinOficio(int seguimiento);
    }
}
