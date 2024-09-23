using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MProyectos
{
    public partial class ProyectosUeg
    {
        public int ProyectoId { get; set; }
        public int UnidadId { get; set; }
        public int Ejercicio { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }
}
