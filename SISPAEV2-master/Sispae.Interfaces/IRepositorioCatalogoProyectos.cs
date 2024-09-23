using Sispae.Entities.MProyectos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioCatalogoProyectos
    {
        Task<List<Proyecto>> GetCatalogoProyectosByUEGTipo(int ueg,string tipo);
        Task<List<Proyecto>> GetCatalogoProyectos();
        Task<List<Proyecto>> GetProyectosSinUEG();
        Task<int> InsertaProyecto(Proyecto proyecto);
        Task<int> actualizaProyecto(Proyecto proyecto);
        Task<Proyecto> ActualizarProyectoById(int proyecto);
    }
}
