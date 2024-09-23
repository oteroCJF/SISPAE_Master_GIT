using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MDashboards
{
    public class DashboardPUEG
    {
        public int Id { get; set; }
        public int NumeroUEG { get; set; }
        public int TotalProyectos { get; set; }
        public int TotalProyectosUEG { get; set; }
        public string Nombre { get; set; }
        public string Icono { get; set; }
        public string Fondo { get; set; }
        public string FondoHexadecimal { get; set; }
    }
}
