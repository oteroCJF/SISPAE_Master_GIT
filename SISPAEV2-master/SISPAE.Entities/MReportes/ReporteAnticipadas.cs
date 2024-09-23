using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MReportes
{
    public partial class ReporteAnticipadas
    {
        public int Capitulo { get; set; }
        public int ClaveProyecto { get; set; }
        public string TipoProyecto { get; set; }
        public string Partidas { get; set; }
        public string Proyecto { get; set; }
        public string FechaEstimada { get; set; }
        public decimal Importe { get; set; }
    }
}
