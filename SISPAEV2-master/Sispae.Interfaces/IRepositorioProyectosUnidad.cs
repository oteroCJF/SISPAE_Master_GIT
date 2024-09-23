using Sispae.Entities.MProyectos;
using Sispae.Entities.Vistas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioProyectosUnidad
    {
        Task<List<VProyectosUEG>> GetProyectosByUEG(int proyecto);
        Task<int> insertaUEGProyecto(List<ProyectosUeg> unidad);
        Task<int> eliminaUEGProyecto(int proyecto, int unidad, int ejercicio);
    }
}
