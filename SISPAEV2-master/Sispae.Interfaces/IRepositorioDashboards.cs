using Sispae.Entities.MDashboards;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioDashboards
    {
        Task<List<Dashboard>> GetDashboardEstatus(int anio);
        Task<List<Dashboard>> GetDashboardEstatusAhorros(int anio);
        Task<List<DashboardPUEG>> GetDashboardProyectosUEG(int anio, string tipo, string estatus);
    }
}
