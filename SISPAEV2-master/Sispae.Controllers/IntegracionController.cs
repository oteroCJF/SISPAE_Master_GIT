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
    public class IntegracionController : Controller
    {
        private readonly IRepositorioIntegracion vIntegracion;
        private readonly IRepositorioPerfiles vPerfil;

        public IntegracionController(IRepositorioPerfiles iPerfil, IRepositorioIntegracion iIntegracion)
        {
            this.vIntegracion = iIntegracion ?? throw new ArgumentNullException(nameof(iIntegracion));
            this.vPerfil = iPerfil ?? throw new ArgumentNullException(nameof(iPerfil));
        }

        [HttpPost]
        [Route("/integracion/integraProyecto")]
        public async Task<IActionResult> NuevaIntegracionProyecto([FromBody] List<Integracion> integracion)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "crear");
            if (success == 1)
            {
                int integra = await vIntegracion.NuevaIntegracionProyecto(integracion); //obtenemos el proyecto a actualizar
                if (integra != -1)
                {
                    return Ok(integra);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

        [HttpPost]
        [Route("/integracion/actualizarIntegracion")]
        public async Task<IActionResult> ActualizarIntegracionProyecto([FromBody] Integracion integracion)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "actualizar integración");
            if (success == 1)
            {
                int integra = await vIntegracion.ActualizarIntegracionProyecto(integracion); //obtenemos el proyecto a actualizar
                if (integra != -1)
                {
                    return Ok(integra);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

        [HttpPost]
        [Route("/integracion/autorizarRechazarInte")]
        public async Task<IActionResult> autorizarRechazarIntegracion([FromBody] Integracion integracion)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "actualizar integración");
            if (success == 1)
            {
                int integra = await vIntegracion.AutorizarRechazarIntegracionProyecto(integracion); //obtenemos el proyecto a actualizar
                if (integra != -1)
                {
                    return Ok(integra);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

        [HttpPost]
        [Route("/integracion/ordenaProyectos")]
        public async Task<IActionResult> ActualizarIntegracionProyecto([FromBody] List<Integracion> integracion)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "actualizar integración");
            if (success == 1)
            {
                int integra = await vIntegracion.ActualizarOrdenProyectos(integracion); //obtenemos el proyecto a actualizar
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
