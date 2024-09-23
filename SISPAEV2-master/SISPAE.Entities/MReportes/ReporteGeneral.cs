using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MReportes
{
    public partial class ReporteGeneral
    {
        public int NumeroUEG { get; set; }
        public string Descripcion { get; set; }
        public int Capitulo2000 { get; set; }
        public int Capitulo3000 { get; set; }
        public int Capitulo5000 { get; set; }
        public decimal TotalRecursos2000 { get; set; }
        public decimal TotalRecursos3000 { get; set; }
        public decimal TotalRecursos5000 { get; set; }
        public int TotalProyectos { get; set; }
        public decimal TotalFinal { get; set; }
    }
}
