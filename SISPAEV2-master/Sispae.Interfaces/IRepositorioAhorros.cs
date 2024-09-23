using Sispae.Entities.Vistas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioAhorros
    {
        Task<VProyectos> GetProyectoById(int id);
    }
}
