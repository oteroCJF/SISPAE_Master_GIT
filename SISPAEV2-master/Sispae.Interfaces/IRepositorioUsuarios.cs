using Sispae.Entities.MUsuarios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioUsuarios
    {
        Task<List<Usuarios>> getUsuarios();
        Task<Usuarios> getUserById(int id);
        Task<int> insertaUsuario(string datosUsuario, string password);
        Task<List<UnidadesUsuarios>> getUnidadesUsuario(int user);
        Task<int> insertaUEGByUser(List<UnidadesUsuarios> uegUsuario);
        Task<int> EliminaUnidadByUser(int id, int user);
        Task<int> asignaPerfil(List<PerfilesUsuario> perfilesUsuario);
        Task<int> actualizaCorreoElectronico(Usuarios usuarios);
    }
}
