using Sispae.Entities.MPrestador;
using Sispae.Entities.MRecursos;
using Sispae.Entities.MUsuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MProyectos
{
    public partial class Seguimiento
    {
        public int Id { get; set; }
        public int IntegracionId { get; set; }
        public int PrestadorId { get; set; }
        public int UsuarioId { get; set; }
        public int Anio{ get; set; }
        public string Contrato { get; set; }
        public string TipoAdjudicacion { get; set; }
        public string Apartado { get; set; }
        public string Mes { get; set; }
        public string Tipo { get; set; }
        public string Estatus { get; set; }
        public string AreaFuncional { get; set; }
        public string Observaciones { get; set; }
        public decimal MontoPropio { get; set; }
        public decimal Monto { get; set; }
        public decimal MontoOtorgado { get; set; }
        public decimal SolicitadoDGPPT { get; set; }
        public decimal Pendiente { get; set; }
        public decimal RecursosDisponibles { get; set; }
        public decimal ImporteEstimado { get; set; }
        public bool Devolver { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime FechaEliminacion { get; set; }
        
        public virtual PrestadorServicios prestador { get; set; }
        public virtual Usuarios usuario { get; set; }
        public virtual List<Recursos> recursos { get; set; }
    }
}
