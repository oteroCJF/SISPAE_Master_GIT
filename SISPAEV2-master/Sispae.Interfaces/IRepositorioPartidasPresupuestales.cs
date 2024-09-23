using Sispae.Entities.MPartidasPresupuestales;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioPartidasPresupuestales
    {
        Task<List<PartidasPresupuestales>> GetPartidasPresupuestales(int integracion);
        Task<int> insertaPartidasByProyecto(PartidasProyecto partidasProyecto);
        Task<List<PartidasPresupuestales>> GetPartidasProyecto(int integracion);
        Task<int> DeletePartidasProyecto(int integracion, int partida);
    }
}
