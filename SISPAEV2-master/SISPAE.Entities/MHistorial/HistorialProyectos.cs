using Sispae.Entities.MUsuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MHistorial
{
    public partial class HistorialProyectos
    {
        public int Id { get; set; }
        public int IntegracionId { get; set; }
        public int UsuarioId { get; set; }
        public string Tipo { get; set; }
        public string Estatus { get; set; }
        public string Comentarios { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

        public virtual Usuarios usuarios { get; set; }
    }
}
