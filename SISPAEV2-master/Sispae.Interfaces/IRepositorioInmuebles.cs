using Sispae.Entities.MInmuebles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioInmuebles
    {
        Task<List<Inmueble>> getAdministraciones();
        Task<Inmueble> inmuebleById(int id);
        Task<int> insertAdmin(Inmueble inmueble);
        Task<int> updateAdmin(Inmueble inmueble);
        Task<int> deleteAdmin(int id);
        Task<List<Inmueble>> getInmueblesByAdmin(int id);
        Task<List<Inmueble>> getEstadosRM();
        Task<List<Inmueble>> getFiltrosInmuebles(int user, int servicio);
    }
}
