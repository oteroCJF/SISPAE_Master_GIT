using Sispae.Entities.MPerfiles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioOperacionesPerfil
    {
        Task<int> insertarOperacionesPerfil(OperacionesPerfil operacionesPerfil);
        Task<List<OperacionesPerfil>> getOperacionesByPerfil(int perfil);
        Task<int> eliminaOpPerfil(int perfil);
        Task<PermisosPerfil> GetPermisoModuloByUser(string permiso, int usuario);
    }
}
