using Sispae.Entities.MModulos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioModulos
    {
        Task<List<Modulos>> getModulos();
    }
}
