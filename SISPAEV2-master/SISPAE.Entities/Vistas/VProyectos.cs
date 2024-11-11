using Sispae.Entities.MEntregables;
using Sispae.Entities.MHistorial;
using Sispae.Entities.MPartidasPresupuestales;
using Sispae.Entities.MPrestador;
using Sispae.Entities.MProyectos;
using Sispae.Entities.MRecursos;
using Sispae.Entities.MRecursosProyecto;
using Sispae.Entities.MTiposProyecto;
using Sispae.Entities.MUEGS;
using Sispae.Entities.MUsuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.Vistas
{
    public partial class VProyectos
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public int UEGId { get; set; }
        public int NumeroUEG { get; set; }
        public int UsuarioId { get; set; }
        public int TipoId { get; set; }
        public string Clasificacion { get; set; }
        public string Proyecto { get; set; }
        public string Icono { get; set; }
        public int Ejercicio { get; set; }
        public int Capitulo { get; set; }
        public int ClaveProyecto { get; set; }
        public int Anio { get; set; }
        public bool Evento{ get; set; }
        public string Adjudica { get; set; }
        public string Mes { get; set; }
        public string Tipo { get; set; }
        public string TipoProyecto { get; set; }
        public string TipoRecurso { get; set; }
        public string Estatus { get; set; }
        public string Fondo { get; set; }
        public decimal Importe { get; set; }
        public decimal RecursosDisponibles { get; set; }
        public decimal Pendiente { get; set; }
        public bool Devolver { get; set; }
        public decimal RecursosRestantes { get; set; }
        public int TotalSeguimientos { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public virtual UEG ueg { get; set; }
        public virtual Usuarios usuarios { get; set; }
        public virtual Seguimiento seguimiento { get; set; }
        public virtual Integracion integracion { get; set; }
        public virtual PrestadorServicios prestador { get; set; }
        public virtual List<Seguimiento> seguimientos { get; set; }
        public virtual List<Integracion> integraciones { get; set; }
        public virtual List<PrestadorServicios> prestadores { get; set; }
        public virtual List<PartidasPresupuestales> partidas { get; set; }
        public virtual List<HistorialProyectos> historial{ get; set; }
        public virtual List<Entregables> entregables{ get; set; }
        public virtual List<CTTipoProyecto> CTTipo { get; set; }
        public virtual CTTipoProyecto Categoria { get; set; }
        public virtual List<VRecursosProyecto> RecursosProyecto { get; set; }
        public virtual List<PartidasPresupuestales> CTPartidas { get; set; }

    }
}
