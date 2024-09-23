using Sispae.Entities.MPrestador;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioPrestadorServicios
    {
        Task<List<PrestadorServicios>> GetCatalogoPrestadores();
        Task<PrestadorServicios> GetPrestadorServiciosById(int id);
    }
}
