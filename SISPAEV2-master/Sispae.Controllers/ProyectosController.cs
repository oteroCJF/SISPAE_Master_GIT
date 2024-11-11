using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sispae.Entities.MPrestador;
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
    [Authorize]
    public class ProyectosController : Controller
    {
        private readonly IRepositorioProyectos vProyectos;
        private readonly IRepositorioUnidadesEjecutoras vUnidad;
        private readonly IRepositorioPartidasPresupuestales vPartidas;
        private readonly IRepositorioSeguimiento vSeguimiento;
        private readonly IRepositorioPrestadorServicios vPrestador;
        private readonly IRepositorioRecursos vRecursos;
        private readonly IRepositorioHistoriales vHistorial;
        private readonly IRepositorioUsuarios vUsuario;
        private readonly IRepositorioPerfiles vPerfil;
        private readonly IRepositorioEntregables vEntregables;
        private readonly IRepositorioCTTipoProyecto vCTTipo;
        private readonly IRepositorioRecursosProyecto vRecursosp;

        public ProyectosController(IRepositorioProyectos iProyectos, IRepositorioProyectosUnidad iPunidad, IRepositorioUnidadesEjecutoras iUnidad, IRepositorioHistoriales iHistorial,
            IRepositorioPartidasPresupuestales iPartidas, IRepositorioUsuarios iUsuario, IRepositorioPerfiles iPerfil, IRepositorioPrestadorServicios iPrestador, 
            IRepositorioSeguimiento iSeguimiento, IRepositorioRecursos iRecursos, IRepositorioEntregables iEntregables,
            IRepositorioCTTipoProyecto iCTTipo, IRepositorioRecursosProyecto iRecursosp)
        {
            this.vProyectos = iProyectos ?? throw new ArgumentNullException(nameof(iProyectos));
            this.vUsuario = iUsuario?? throw new ArgumentNullException(nameof(iUsuario));
            this.vUnidad = iUnidad ?? throw new ArgumentNullException(nameof(iUnidad));
            this.vSeguimiento = iSeguimiento ?? throw new ArgumentNullException(nameof(iSeguimiento));
            this.vPartidas = iPartidas ?? throw new ArgumentNullException(nameof(iPartidas));
            this.vPrestador = iPrestador ?? throw new ArgumentNullException(nameof(iPrestador));
            this.vEntregables = iEntregables ?? throw new ArgumentNullException(nameof(iEntregables));
            this.vRecursos = iRecursos ?? throw new ArgumentNullException(nameof(iRecursos));
            this.vHistorial = iHistorial ?? throw new ArgumentNullException(nameof(iHistorial));
            this.vPerfil = iPerfil ?? throw new ArgumentNullException(nameof(iPerfil));
            this.vCTTipo = iCTTipo ?? throw new ArgumentNullException(nameof(iCTTipo));
            this.vRecursosp = iRecursosp ?? throw new ArgumentNullException(nameof(iRecursosp));
        }

        [Route("/proyectos/index")]
        public async Task<IActionResult> Index([FromQuery(Name = "Anio")] int anio, [FromQuery(Name = "Catalogo")] string catalogo, [FromQuery(Name = "UEG")] 
        int UEG, [FromQuery(Name = "Estatus")] string estatus, [FromQuery(Name = "Ordenar")] bool ordenar)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "ver");
            if (success == 1)
            {
                Models models = new Models();
                models.Anio = anio;
                models.ueg = UEG;
                models.Estatus = estatus;
                models.Catalogo = catalogo;
                if (anio != 0 && !catalogo.Equals(""))
                {
                    models.uegsAsignadas = await vUnidad.GetUnidadesEjecutorasByUser(UserId());
                }

                if (UEG != 0)
                {
                    models.uegSeleccionada = await vUnidad.GetUnidadEjecutoraById(UEG);
                    models.proyectosEstatus = await vProyectos.GetProyectosByUEG(UEG,anio,UserId(),catalogo);
                }

                if (estatus != null)
                {
                    models.Ordenar = ordenar;
                    models.lProyectos = await vProyectos.GetProyectosEstatusByUEG(UEG,estatus, anio, UserId(), catalogo);
                }

                return View(models);
            }
            return Redirect("/error/denied");
        }

        [Route("/proyectos/integracion/{id}")]
        public async Task<IActionResult> Integracion(int id)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "crear");
            VProyectos proyecto = null;
            if (success == 1)
            {
                proyecto = await vProyectos.GetProyectoById(id);
                proyecto.ueg = await vUnidad.GetUnidadEjecutoraById(proyecto.UEGId);
                proyecto.entregables = await vEntregables.getEntregablesMemorias(id);
                proyecto.CTTipo = await vCTTipo.GetCTTiposProyecto();
                proyecto.partidas = await vPartidas.GetPartidasProyecto(id);
                proyecto.CTPartidas = await vPartidas.GetPartidasPresupuestales(id);
                proyecto.RecursosProyecto = await vRecursosp.GetRecursosProyecto(id);
                return View(proyecto);
            }
            return Redirect("/error/denied");
        }

        [Route("/proyectos/revisar/{id}")]
        public async Task<IActionResult> RevisarIntegracion(int id)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "revisar");
            VProyectos proyecto = null;
            if (success == 1)
            {
                proyecto = await vProyectos.GetProyectoById(id);
                proyecto.ueg = await vUnidad.GetUnidadEjecutoraById(proyecto.UEGId);
                proyecto.usuarios = await vUsuario.getUserById(proyecto.UsuarioId);
                proyecto.partidas = await vPartidas.GetPartidasProyecto(id);
                proyecto.Categoria= await vCTTipo.GetCategoriaById(proyecto.TipoId);
                proyecto.RecursosProyecto = await vRecursosp.GetRecursosProyecto(id);
                proyecto.historial = await vHistorial.getHistorialByProyecto(id);
                foreach (var user in proyecto.historial)
                {
                    user.usuarios = await vUsuario.getUserById(user.UsuarioId);
                }
                return View(proyecto);
            }
            return Redirect("/error/denied");
        }

        /*************************************** Vistas Adjudicación y Seguimiento *************************************************/
        [Route("/proyectos/adjudicacion/{id}")]
        public async Task<IActionResult> AdjudicarProyecto(int id)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            VProyectos proyecto = null;
            if (success == 1)
            {
                proyecto = await vProyectos.GetProyectoById(id);
                proyecto.ueg = await vUnidad.GetUnidadEjecutoraById(proyecto.UEGId);
                proyecto.prestadores = await vPrestador.GetCatalogoPrestadores();
                return View(proyecto);
            }
            return Redirect("/error/denied");
        }

        [Route("/proyectos/detalleAdjudicacion/{integracion}/{seguimiento}")]
        public async Task<IActionResult> DetalleAdjudicacion(int integracion, int seguimiento)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            VProyectos proyecto = null;
            if (success == 1)
            {
                proyecto = await vProyectos.GetProyectoById(integracion);
                proyecto.ueg = await vUnidad.GetUnidadEjecutoraById(proyecto.UEGId);
                proyecto.entregables = await vEntregables.getEntregables(seguimiento);
                proyecto.seguimiento = await vSeguimiento.getSeguimientoById(seguimiento);
                proyecto.prestador = await vPrestador.GetPrestadorServiciosById(proyecto.seguimiento.PrestadorId);
                proyecto.seguimiento.recursos = await vRecursos.getRecursosBySeguimiento(seguimiento);
                return View(proyecto);
            }
            return Redirect("/error/denied");
        }

        [Route("/proyectos/actualizar/adjudicacion/{integracion}/{seguimiento}")]
        public async Task<IActionResult> AdjudicarProyecto(int integracion, int seguimiento)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            VProyectos proyecto = null;
            if (success == 1)
            {
                proyecto = await vProyectos.GetProyectoById(integracion);
                proyecto.ueg = await vUnidad.GetUnidadEjecutoraById(proyecto.UEGId);
                proyecto.entregables = await vEntregables.getEntregables(seguimiento);
                proyecto.prestadores = await vPrestador.GetCatalogoPrestadores();
                proyecto.seguimiento = await vSeguimiento.getSeguimientoById(seguimiento);
                proyecto.seguimiento.recursos = await vRecursos.getRecursosBySeguimiento(seguimiento);
                return View(proyecto);
            }
            return Redirect("/error/denied");
        }

        [Route("/proyectos/seguimiento/{id}")]
        public async Task<IActionResult> SeguimientoProyecto(int id)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "seguimiento");
            VProyectos proyecto = null;
            if (success == 1)
            {
                proyecto = await vProyectos.GetProyectoById(id);
                proyecto.ueg = await vUnidad.GetUnidadEjecutoraById(proyecto.UEGId);
                proyecto.usuarios = await vUsuario.getUserById(proyecto.UsuarioId);
                proyecto.partidas = await vPartidas.GetPartidasProyecto(id);
                proyecto.seguimientos = await vSeguimiento.getSeguimientoByIntegracion(id);
                foreach (var segui in proyecto.seguimientos)
                {
                    segui.prestador = await vPrestador.GetPrestadorServiciosById(segui.PrestadorId);
                    segui.usuario = await vUsuario.getUserById(segui.UsuarioId);
                }
                proyecto.historial = await vHistorial.getHistorialByProyecto(id);
                foreach (var user in proyecto.historial)
                {
                    user.usuarios = await vUsuario.getUserById(user.UsuarioId);
                }
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
