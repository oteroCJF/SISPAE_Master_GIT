using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.Vistas
{
    public partial class VProyectosUEG
    {
        public int UnidadId { get; set; }
        public int Ejercicio { get; set; }
        public int NumeroUEG { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }
}
