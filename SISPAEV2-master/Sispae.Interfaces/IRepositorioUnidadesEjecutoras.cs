using Sispae.Entities.MUEGS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioUnidadesEjecutoras
    {
        Task<List<UEG>> GetUnidadesEjecutoras();
        Task<UEG> GetUnidadEjecutoraById(int id);
        Task<int> UpdateUnidadEjecutora(UEG ueg);
        Task<int> DeleteUnidadEjecutora(int id);
        Task<List<UEG>> GetUnidadesEjecutorasByUser(int user);
    }
}
