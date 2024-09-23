using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MProyectos
{
    public partial class Proyecto
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime FechaEliminacion { get; set; }
    }
}
