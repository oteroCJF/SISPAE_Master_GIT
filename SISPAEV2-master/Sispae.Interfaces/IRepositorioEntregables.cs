using Sispae.Entities.MEntregables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioEntregables
    {
        Task<List<Entregables>> getEntregables(int seguimiento);
        Task<List<Entregables>> getEntregablesMemorias(int seguimiento);
        Task<int> insertaEntregable(Entregables entregable);
        Task<int> insertaEntregableMemoria(Entregables entregable);
        Task<int> actualizaEntregable(Entregables entregable);
        Task<int> eliminaEntregable(Entregables entregable);
        Task<int> eliminaEntregables(Entregables entregable);
        Task<int> eliminaEntregableMemoria(Entregables entregable);
    }
}
