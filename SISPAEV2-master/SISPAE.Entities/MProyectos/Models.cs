using Sispae.Entities.MDashboards;
using Sispae.Entities.MUEGS;
using Sispae.Entities.MUsuarios;
using Sispae.Entities.Vistas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MProyectos
{
    public partial class Models
    {
        public List<UEG> uegsAsignadas { get; set; } // UEG asignadas al usuario
        public UEG uegSeleccionada { get; set; } // UEG asignadas al usuario
        public List<VProyectosUEG> uegs { get; set; } //Proyectos asignados a las UEGS
        public List<DashboardProyectos> proyectosEstatus { get; set; }
        public List<VProyectos> lProyectos { get; set; }
        public Proyecto proyecto { get; set; }
        public List<Proyecto> proyectos { get; set; }
        public List<Proyecto> proyectoSUEG { get; set; }
        public int ueg { get; set; }
        public int Anio { get; set; }
        public string Estatus { get; set; }
        public string Catalogo { get; set; }
        public string Reporte { get; set; }
        public string Presupuesto { get; set; }
        public bool Ordenar { get; set; }
    }
}
