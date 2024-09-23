using Sispae.Entities.MReportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Interfaces
{
    public interface IRepositorioReportesUEG
    {
        Task<List<ReporteGeneral>> GetReporteGeneralUEG(int ejercicio, string presupuesto);
        Task<List<ReporteGeneral>> GetReporteGeneralAhorrosUEG(int ejercicio, string presupuesto);
        Task<List<ReporteUEG>> GetProyectosUEG(int ueg, int ejercicio, int capitulo,string presupuesto,int id);
        Task<List<ReporteUEG>> GetProyectosByUEG(int ueg, int ejercicio, string presupuesto);
        Task<List<ReportePunto>> GetReportePunto(int ejercicio);
        Task<List<ReportePuntoAhorros>> GetReportePuntoAhorros(int ejercicio);
        Task<List<ReportePunto>> GetReportePuntoByUEG(int ejercicio,int ueg);
        Task<List<ReportePuntoAhorros>> GetReportePuntoAhorrosByUEG(int ejercicio, int ueg);
        Task<List<ReporteCalendarioUEG>> GetReporteCalendarioUEG(int ejercicio, int ueg);
        Task<List<ReporteAnticipadas>> GetReporteAnticipadas(int ejercicio, int ueg, int capitulo, int id);
        Task<List<ReportePunto>> GetReporteGeneralTP(int ejercicio);
        Task<List<ReporteCalendario>> GetCalendarizacion();
    }
}
