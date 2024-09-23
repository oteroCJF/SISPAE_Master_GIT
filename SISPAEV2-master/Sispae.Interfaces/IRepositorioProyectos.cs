using Sispae.Entities.MDashboards;
using Sispae.Entities.MProyectos;
using Sispae.Entities.Vistas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioProyectos
    {
        Task<List<DashboardProyectos>> GetProyectosByUEG(int ueg, int ejercicio, int usuario, string catalogo);
        Task<List<VProyectos>> GetProyectosEstatusByUEG(int ueg, string estatus, int ejercicio, int usuario, string catalogo);
        Task<VProyectos> GetProyectoById(int id);
    }
}
