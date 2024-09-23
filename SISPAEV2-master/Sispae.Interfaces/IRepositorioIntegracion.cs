using Sispae.Entities.MProyectos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioIntegracion
    {
        Task<int> NuevaIntegracionProyecto(List<Integracion> integracion);
        Task<int> AutorizarRechazarIntegracionProyecto(Integracion integracion);
        Task<int> ActualizarIntegracionProyecto(Integracion integracion);
        Task<int> ActualizarOrdenProyectos(List<Integracion> ordenamiento);
    }
}
