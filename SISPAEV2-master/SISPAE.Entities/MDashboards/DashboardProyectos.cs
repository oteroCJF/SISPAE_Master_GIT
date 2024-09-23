using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MDashboards
{
    public partial class DashboardProyectos
    {
        public int UEGId { get; set; }
        public int Ejercicio{ get; set; }
        public string Estatus {get; set; }
        public string Icono {get; set; }
        public string Fondo {get; set; }
        public int TotalEstatus { get; set; }

    }
}
