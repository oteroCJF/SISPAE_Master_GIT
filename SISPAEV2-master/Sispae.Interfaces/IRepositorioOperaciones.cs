using Sispae.Entities.MPerfiles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioOperaciones
    {
        Task<List<Operaciones>> getOperaciones();
    }
}
