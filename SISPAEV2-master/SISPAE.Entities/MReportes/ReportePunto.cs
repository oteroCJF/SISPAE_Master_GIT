using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MReportes
{
    public partial class ReportePunto
    {
        public int Id { get; set; }
        public int NumeroUEG { get; set; }
        public int Capitulo { get; set; }
        public int NoProyecto { get; set; }
        public int ClaveProyecto { get; set; }
        public int TipoId { get; set; }
        public int ColumnaTramite { get; set; }
        public string Descripcion { get; set; }
        public string Partidas { get; set; }
        public string Proyecto { get; set; }
        public string Nombre { get; set; }
        public string Trimestre { get; set; }
        public string Presupuesto { get; set; }
        public decimal Importe { get; set; }
        public string FechaEstimada { get; set; }
        public string FechaAdjudicacion { get; set; }
        public decimal MontoPropio { get; set; }
        public decimal ImporteTotal { get; set; }
        public decimal MontoAdjudicado{ get; set; }
        public decimal Tramite { get; set; }
        public decimal TramiteCovid { get; set; }
        public decimal RecursosDisponibles { get; set; }
        public decimal RecursosUtilizar { get; set; }
        public decimal TesoreriaDGRM  { get; set; }
        public decimal SuficienciaDGPPT { get; set; }
        public decimal SuficienciaDGRM { get; set; }
        public decimal DTesoreria { get; set; }
        public string OficiosTesoreria { get; set; }
        public string FechasTesoreria { get; set; }
        public decimal DTCovid { get; set; }
        public string OficiosTCovid{ get; set; }
        public string FechasTCovid { get; set; }
        public decimal DDeductiva { get; set; }
        public string OficiosDeductivas { get; set; }
        public string FechasDeductivas { get; set; }
        public string Prestador { get; set; }
        public string Contrato { get; set; }
        public string TipoAdjudicacion { get; set; }
        public string Observaciones { get; set; }
        public string Apartado { get; set; }
        public string Tipo { get; set; }

    }
}
