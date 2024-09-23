using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MTiposProyecto
{
    public class CTTipoProyecto
    {
        public int Id { get; set; }
        public string Abreviacion { get; set; }
        public string Siglas { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
