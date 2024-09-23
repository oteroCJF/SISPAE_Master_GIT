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
    public class SeguimientoController : Controller
    {
        private readonly IRepositorioSeguimiento vSeguimiento;
        private readonly IRepositorioPerfiles vPerfil;

        public SeguimientoController(IRepositorioSeguimiento iSeguimiento, IRepositorioPerfiles iPerfil)
        {
            this.vSeguimiento = iSeguimiento ?? throw new ArgumentNullException(nameof(iSeguimiento));
            this.vPerfil = iPerfil ?? throw new ArgumentNullException(nameof(iPerfil));
        }

        [HttpPost]
        [Route("/seguimiento/insertaSeguimiento")]
        public async Task<IActionResult> InsertarSeguimiento([FromBody] Seguimiento seguimiento)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            if (success == 1)
            {
                int integra = await vSeguimiento.insertaSeguimiento(seguimiento); //obtenemos el proyecto a actualizar
                 if (integra != -1)
                {
                    return Ok(integra);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

        [HttpPost]
        [Route("/seguimiento/actualizarSeguimiento")]
        public async Task<IActionResult> ActualizarSeguimiento([FromBody] Seguimiento seguimiento)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            if (success == 1)
            {
                int integra = await vSeguimiento.actualizaSeguimiento(seguimiento); //obtenemos el proyecto a actualizar
                if (integra != -1 && integra != 0)
                {
                    return Ok(integra);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

        [HttpGet]
        [Route("/seguimiento/eliminaSeguimiento/{id}")]
        public async Task<IActionResult> eliminaSeguimiento(int id)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "eliminar seguimiento");
            if (success == 1)
            {
                int integra = await vSeguimiento.eliminaSeguimiento(id); //obtenemos el proyecto a actualizar
                if (integra != -1 && integra != 0)
                {
                    return Ok(integra);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

        [HttpPost]
        [Route("/seguimiento/enviaSeguimiento")]
        public async Task<IActionResult> EnviaSeguimiento([FromBody] Seguimiento seguimiento)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            if (success == 1)
            {
                int envia = await vSeguimiento.enviaSeguimiento(seguimiento); //obtenemos el proyecto a actualizar
                if (envia != -1 && envia != 0)
                {
                    return Ok(envia);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

        [HttpPost]
        [Route("/seguimiento/autorizaRechazaSeguimiento")]
        public async Task<IActionResult> AutorizaRechazaSeguimiento([FromBody] Seguimiento seguimiento)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "autorizar seguimiento");
            if (success == 1)
            {
                int autoriza = await vSeguimiento.autorizaRechazaSeguimiento(seguimiento); //obtenemos el proyecto a actualizar
                if (autoriza != -1)
                {
                    return Ok(autoriza);
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
