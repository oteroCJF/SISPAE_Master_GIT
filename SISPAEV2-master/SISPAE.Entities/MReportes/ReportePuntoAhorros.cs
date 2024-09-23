using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MReportes
{
    public partial class ReportePuntoAhorros
    {
        public int Id { get; set; }
        public int NumeroUEG { get; set; }
        public int Trimestre { get; set; }
        public int ClaveProyecto { get; set; }
        public string Descripcion { get; set; }
        public string Partidas { get; set; }
        public string Proyecto { get; set; }
        public decimal Importe { get; set; }
        public int OficiosSolicitud { get; set; }
        public bool Evento { get; set; }
        public string NombreOficioSolicitud { get; set; }
        public decimal RecursosSolicitados { get; set; }
        public string FechasSolicitud { get; set; }
        public string AreaFuncional { get; set; }
        public string OficiosOtorgados { get; set; }
        public string NombreOficioOtorgado { get; set; }
        public decimal RecursosOtorgados { get; set; }
        public string FechasOtorgados { get; set; }
        public string FechaAdjudicacion { get; set; }
        public decimal Monto { get; set; }
        public decimal RecursosUtilizar { get; set; }
        public string Nombre { get; set; }
        public string Prestador { get; set; }
        public string Contrato { get; set; }
        public decimal Variacion { get; set; }
        public decimal DTesoreria { get; set; }
        public string OficiosTesoreria { get; set; }
        public string FechasTesoreria { get; set; }
        public string NombreOficioDGPPT { get; set; }
        public decimal DDeductiva { get; set; }
        public string OficiosDeductivas { get; set; }
        public string FechasDeductivas { get; set; }
        public string Observaciones { get; set; }
    }
}
