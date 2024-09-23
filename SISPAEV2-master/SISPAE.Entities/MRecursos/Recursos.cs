using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MRecursos
{
    public class Recursos
    {
        public int Id { get; set; }
        public int SeguimientoId { get; set; }
        public long NumeroOficio { get; set; }
        public string Oficio { get; set; }
        public string Tipo { get; set; }
        public string TipoRecurso { get; set; }
        public DateTime FechaOficio { get; set; }
        public decimal Monto { get; set; }
        public IFormFile Archivo { get; set; }
        public string NombreArchivo { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime FechaEliminacion { get; set; }
    }
}
