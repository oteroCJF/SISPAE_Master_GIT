using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MEntregables
{
    public class Entregables
    {
        public int Id { get; set; }
        public int SeguimientoId { get; set; }
        public int IntegracionId { get; set; }
        public int UsuarioId { get; set; }
        public string Tipo { get; set; }
        public IFormFile Archivo { get; set; }
        public string NombreArchivo { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime FechaEliminacion { get; set; }

    }
}
