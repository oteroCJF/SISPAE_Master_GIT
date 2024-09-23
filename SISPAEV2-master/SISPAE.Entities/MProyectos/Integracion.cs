using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MProyectos
{
    public partial class Integracion
    {
        public int Id { get; set; }
        public int UEGId { get; set; }
        public int ProyectoId { get; set; }
        public int UsuarioId { get; set; }
        public int Ejercicio { get; set; }
        public int Capitulo { get; set; }
        public int ClaveProyecto { get; set; }
        public int Anio { get; set; }
        public int TipoId { get; set; }
        public bool Evento { get; set; }
        public string Mes { get; set; }
        public string Tipo { get; set; }
        public string TipoProyecto { get; set; }
        public string Estatus { get; set; }
        public decimal Importe { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime FechaEliminacion { get; set; }
    }
}
