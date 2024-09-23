using Sispae.Entities.MPerfiles;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Services
{
    public class PermisosService
    {
        private readonly IRepositorioOperacionesPerfil vPermisos;

        public PermisosService(IRepositorioOperacionesPerfil viPermisos)
        {
            this.vPermisos = viPermisos ?? throw new ArgumentNullException(nameof(viPermisos));
        }

        public async Task<int> GetPermisosByModulo(string permiso, int usuario)
        {
            PermisosPerfil modulos = await vPermisos.GetPermisoModuloByUser(permiso, usuario);
            return modulos.Id;
        }
    }
}
