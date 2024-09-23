using Sispae.Entities.MPerfiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MModulos
{
    public partial class Modulos
    {
        public Modulos()
        {
            Operaciones = new HashSet<Operaciones>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public virtual ICollection<Operaciones> Operaciones { get; set; }
    }
}
