using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MDashboards
{
    public partial class DModels
    {
        public int Anio { get; set; }
        public string Tipo { get; set; }
        public string Estatus { get; set; }
        public List<Dashboard> dashboardEstatus { get; set; }
        public List<Dashboard> dashboardEstatusA { get; set; }
        public List<DashboardPUEG> dashboardpueg { get; set; }
    }
}
