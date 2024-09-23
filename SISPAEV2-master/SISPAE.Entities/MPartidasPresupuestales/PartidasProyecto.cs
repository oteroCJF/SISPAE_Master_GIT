using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MPartidasPresupuestales
{
    public partial class PartidasProyecto
    {
        public int PartidaId { get; set; }
        public int IntegracionId { get; set; }
        public decimal Monto { get; set; }
    }
}
