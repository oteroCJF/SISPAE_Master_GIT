using Sispae.Entities.MLogin;
using Sispae.Entities.Vistas;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Services
{
    public class ModuleService
    {
        private readonly IRepositorioLogin vLogin;

        public ModuleService(IRepositorioLogin iRepositorioLogin)
        {
            this.vLogin = iRepositorioLogin ?? throw new ArgumentNullException(nameof(iRepositorioLogin));
        }

        public async Task<List<VModulosUsuario>> GetVModulos(int user)
        {
            List<VModulosUsuario> modulos = await vLogin.getModulosByUser(user);
            return modulos;
        }

        public async Task<List<ResponsablesDAS>> GetResponsablesDAS()
        {
            List<ResponsablesDAS> responsables = await vLogin.GetResponsablesDAS();
            return responsables;
        }


    }
}
