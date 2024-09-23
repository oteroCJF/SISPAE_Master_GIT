using Sispae.Entities.MUEGS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MReportes
{
    public partial class ReporteUEG
    {
        public int Id { get;set;}
        public int UEGId { get;set;}
        public int Capitulo { get;set;}
        public string Presupuesto { get;set;}
        public string Proyecto { get;set;}
        public string Partidas { get;set;}
        public string FechaEstimada { get;set;}
        public decimal Importe { get;set;}
        public string Importes { get;set;}
        public UEG ueg { get;set;}

    }
}
