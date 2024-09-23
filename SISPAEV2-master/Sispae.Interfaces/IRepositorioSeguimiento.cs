using Sispae.Entities.MProyectos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioSeguimiento
    {
        Task<List<Seguimiento>> getSeguimientoByIntegracion(int integracion);
        Task<Seguimiento> getSeguimientoById(int seguimiento);
        Task<int> insertaSeguimiento(Seguimiento seguimiento);
        Task<int> actualizaSeguimiento(Seguimiento seguimiento);
        Task<int> eliminaSeguimiento(int id);
        Task<int> enviaSeguimiento(Seguimiento seguimiento);
        Task<int> autorizaRechazaSeguimiento(Seguimiento seguimiento);
    }
}
