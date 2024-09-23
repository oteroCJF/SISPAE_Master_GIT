using Sispae.Entities.MTiposProyecto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioCTTipoProyecto
    {
        Task<List<CTTipoProyecto>> GetCTTiposProyecto();
        Task<CTTipoProyecto> GetCategoriaById(int id);
    }
}
