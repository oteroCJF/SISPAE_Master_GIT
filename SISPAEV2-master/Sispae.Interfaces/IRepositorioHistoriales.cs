using Sispae.Entities.MHistorial;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioHistoriales
    {
        Task<List<HistorialProyectos>> getHistorialByProyecto(int proyecto);

        Task<int> capturaHistorial(HistorialProyectos historial);
    }
}
