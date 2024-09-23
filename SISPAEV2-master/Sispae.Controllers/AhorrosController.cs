using Microsoft.AspNetCore.Mvc;
using Sispae.Entities.MProyectos;
using Sispae.Entities.Vistas;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Controllers
{
    public class AhorrosController : Controller
    {

        private readonly IRepositorioAhorros vAhorros;
        private readonly IRepositorioIntegracion vIntegracion;
        private readonly IRepositorioProyectosUnidad vPunidad;
        private readonly IRepositorioUnidadesEjecutoras vUnidad;
        private readonly IRepositorioPartidasPresupuestales vPartidas;
        private readonly IRepositorioSeguimiento vSeguimiento;
        private readonly IRepositorioPrestadorServicios vPrestador;
        private readonly IRepositorioRecursos vRecursos;
        private readonly IRepositorioHistoriales vHistorial;
        private readonly IRepositorioUsuarios vUsuario;
        private readonly IRepositorioPerfiles vPerfil;
        private readonly IRepositorioEntregables vEntregables;

        public AhorrosController(IRepositorioAhorros iAhorros, IRepositorioProyectosUnidad iPunidad, IRepositorioUnidadesEjecutoras iUnidad, IRepositorioHistoriales iHistorial,
            IRepositorioPartidasPresupuestales iPartidas, IRepositorioUsuarios iUsuario, IRepositorioPerfiles iPerfil, IRepositorioPrestadorServicios iPrestador,
            IRepositorioSeguimiento iSeguimiento, IRepositorioRecursos iRecursos, IRepositorioEntregables iEntregables, IRepositorioIntegracion iIntegracion)
        {
            this.vAhorros = iAhorros ?? throw new ArgumentNullException(nameof(iAhorros));
            this.vIntegracion = iIntegracion ?? throw new ArgumentNullException(nameof(iIntegracion));
            this.vUsuario = iUsuario ?? throw new ArgumentNullException(nameof(iUsuario));
            this.vUnidad = iUnidad ?? throw new ArgumentNullException(nameof(iUnidad));
            this.vSeguimiento = iSeguimiento ?? throw new ArgumentNullException(nameof(iSeguimiento));
            this.vPartidas = iPartidas ?? throw new ArgumentNullException(nameof(iPartidas));
            this.vPrestador = iPrestador ?? throw new ArgumentNullException(nameof(iPrestador));
            this.vEntregables = iEntregables ?? throw new ArgumentNullException(nameof(iEntregables));
            this.vRecursos = iRecursos ?? throw new ArgumentNullException(nameof(iRecursos));
            this.vHistorial = iHistorial ?? throw new ArgumentNullException(nameof(iHistorial));
            this.vPunidad = iPunidad ?? throw new ArgumentNullException(nameof(iPunidad));
            this.vPerfil = iPerfil ?? throw new ArgumentNullException(nameof(iPerfil));
        }

        [Route("/ahorros/integracion/{id}")]
        public async Task<IActionResult> Integracion(int id)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "crear");
            VProyectos proyecto = null;
            if (success == 1)
            {
                proyecto = await vAhorros.GetProyectoById(id);
                proyecto.ueg = await vUnidad.GetUnidadEjecutoraById(proyecto.UEGId);
                proyecto.partidas = await vPartidas.GetPartidasProyecto(id);
                proyecto.CTPartidas = await vPartidas.GetPartidasPresupuestales(id);
                return View(proyecto);
            }
            return Redirect("/error/denied");
        }

        /*************************************** Vistas Adjudicación y Seguimiento *************************************************/
        [Route("/ahorros/adjudicacion/{id}")]
        public async Task<IActionResult> AdjudicarProyecto(int id)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            VProyectos proyecto = null;
            if (success == 1)
            {
                proyecto = await vAhorros.GetProyectoById(id);
                proyecto.ueg = await vUnidad.GetUnidadEjecutoraById(proyecto.UEGId);
                proyecto.prestadores = await vPrestador.GetCatalogoPrestadores();
                proyecto.partidas = await vPartidas.GetPartidasProyecto(id);
                proyecto.CTPartidas = await vPartidas.GetPartidasPresupuestales(id);
                return View(proyecto);
            }
            return Redirect("/error/denied");
        }

        [Route("/ahorros/actualizar/adjudicacion/{integracion}/{seguimiento}")]
        public async Task<IActionResult> AdjudicarProyecto(int integracion, int seguimiento)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            VProyectos proyecto = null;
            if (success == 1)
            {
                proyecto = await vAhorros.GetProyectoById(integracion);
                proyecto.ueg = await vUnidad.GetUnidadEjecutoraById(proyecto.UEGId);
                proyecto.entregables = await vEntregables.getEntregables(seguimiento);
                proyecto.prestadores = await vPrestador.GetCatalogoPrestadores();
                proyecto.seguimiento = await vSeguimiento.getSeguimientoById(seguimiento);
                proyecto.seguimiento.recursos = await vRecursos.getRecursosBySeguimiento(seguimiento);
                return View(proyecto);
            }
            return Redirect("/error/denied");
        }

        [Route("/ahorros/detalleAdjudicacion/{integracion}/{seguimiento}")]
        public async Task<IActionResult> DetalleAdjudicacion(int integracion, int seguimiento)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            VProyectos proyecto = null;
            if (success == 1)
            {
                proyecto = await vAhorros.GetProyectoById(integracion);
                proyecto.ueg = await vUnidad.GetUnidadEjecutoraById(proyecto.UEGId);
                proyecto.entregables = await vEntregables.getEntregables(seguimiento);
                proyecto.seguimiento = await vSeguimiento.getSeguimientoById(seguimiento);
                proyecto.prestador = await vPrestador.GetPrestadorServiciosById(proyecto.seguimiento.PrestadorId);
                proyecto.seguimiento.recursos = await vRecursos.getRecursosBySeguimiento(seguimiento);
                return View(proyecto);
            }
            return Redirect("/error/denied");
        }

        /************************************* Fin Vistas Adjudicación y Seguimiento ***********************************************/

        private int UserId()
        {
            return Convert.ToInt32(User.Claims.ElementAt(0).Value);
        }

        private string modulo()
        {
            return "Proyectos";
        }
    }
}
