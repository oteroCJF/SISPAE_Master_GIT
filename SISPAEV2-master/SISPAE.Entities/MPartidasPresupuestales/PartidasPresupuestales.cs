using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MPartidasPresupuestales
{
    public partial class PartidasPresupuestales
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Monto { get; set; }

    }
}
