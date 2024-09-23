using Microsoft.AspNetCore.Mvc;
using Sispae.Entities.MProyectos;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Controllers
{
    public class CatalogoProyectosController : Controller
    {
        private readonly IRepositorioCatalogoProyectos vCatalogo;
        private readonly IRepositorioProyectosUnidad vPunidad;
        private readonly IRepositorioUnidadesEjecutoras vUnidad;
        private readonly IRepositorioUsuarios vUsuario;
        private readonly IRepositorioPerfiles vPerfil;

        public CatalogoProyectosController(IRepositorioCatalogoProyectos iCatalogo, IRepositorioProyectosUnidad iPunidad, IRepositorioUnidadesEjecutoras iUnidad,
            IRepositorioUsuarios iUsuario, IRepositorioPerfiles iPerfil)
        {
            this.vCatalogo = iCatalogo ?? throw new ArgumentNullException(nameof(iCatalogo));
            this.vPunidad = iPunidad ?? throw new ArgumentNullException(nameof(iPunidad));
            this.vUnidad = iUnidad ?? throw new ArgumentNullException(nameof(iUnidad));
            this.vUsuario = iUsuario ?? throw new ArgumentNullException(nameof(iUsuario));
            this.vPerfil = iPerfil ?? throw new ArgumentNullException(nameof(iPerfil));
        }

        [Route("/proyectos/catalogo")]
        public async Task<IActionResult> Catalogo([FromQuery(Name = "UEG")] int ueg, [FromQuery(Name = "Tipo")] string tipo)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "catálogo");
            Models models = new Models();
            if (success == 1)
            {
                models.uegsAsignadas = await vUnidad.GetUnidadesEjecutorasByUser(UserId());
                models.ueg = ueg;
                if (ueg != 0 && (tipo != null && !tipo.Equals("")))
                {
                    models.Catalogo = tipo;
                    models.proyectos = await vCatalogo.GetCatalogoProyectosByUEGTipo(ueg, tipo);
                }
                else if (models.ueg == 0)
                {
                    models.proyectoSUEG = await vCatalogo.GetProyectosSinUEG();
                }
                return View(models);
            }
            return Redirect("/error/denied");
        }

        [Route("/proyectos/catalogo/detalle/{proyecto}")]
        public async Task<IActionResult> DetalleProyecto(int proyecto)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "actualizar");
            Models models = new Models();
            if (success == 1)
            {
                models.proyecto = await vCatalogo.ActualizarProyectoById(proyecto); //obtenemos el proyecto a actualizar
                models.uegs = await vPunidad.GetProyectosByUEG(proyecto);
                return View(models);
            }
            return Redirect("/error/denied");
        }

        [HttpPost]
        [Route("/proyectos/nuevoProyecto")]
        public async Task<IActionResult> NuevaIntegracionProyecto([FromBody] Proyecto proyecto)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "crear");
            if (success == 1)
            {
                int integra = await vCatalogo.InsertaProyecto(proyecto); //obtenemos el proyecto a actualizar
                if (integra != -1)
                {
                    return Ok(integra);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

        [HttpPost]
        [Route("/proyectos/actualizarProyecto")]
        public async Task<IActionResult> ActualizarIntegracionProyecto([FromBody] Proyecto proyecto)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "actualizar integración");
            if (success == 1)
            {
                int integra = await vCatalogo.actualizaProyecto(proyecto); //obtenemos el proyecto a actualizar
                if (integra != -1)
                {
                    return Ok(integra);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

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
