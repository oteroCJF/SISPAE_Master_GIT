using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MUsuarios
{
    public partial class UnidadesUsuarios
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int UnidadId { get; set; }
        public int? Clave { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }
}
