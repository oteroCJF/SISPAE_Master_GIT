using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MUEGS
{
    public partial class UEG
    {
        public int Id { get; set; }
        public int Orden { get; set; }
        public int NumeroUEG { get; set; }
        public string Nombre { get; set; }
        public string Descripcion{ get; set; }
        public string Fondo { get; set; }
        public string FondoHexadecimal { get; set; }
        public string Icono { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime FechaEliminacion{ get; set; }
    }
}
