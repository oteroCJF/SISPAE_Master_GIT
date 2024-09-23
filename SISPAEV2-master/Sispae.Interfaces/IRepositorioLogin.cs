using Sispae.Entities.MDashboards;
using Sispae.Entities.MLogin;
using Sispae.Entities.Vistas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioLogin
    {
        Task<int> buscaUsuario(string usuario, string password);
        Task<DatosUsuario> login(string usuario, string password);
        Task<List<VModulosUsuario>> getModulosByUser(int user);
        Task<List<ResponsablesDAS>> GetResponsablesDAS();
    }
}
