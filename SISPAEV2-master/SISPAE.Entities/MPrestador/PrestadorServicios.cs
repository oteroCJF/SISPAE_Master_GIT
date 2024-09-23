using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MPrestador
{
    public partial class PrestadorServicios
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion{ get; set; }
        public DateTime FechaEliminacion { get; set; }
    }
}
