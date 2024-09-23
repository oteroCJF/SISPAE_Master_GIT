using Microsoft.AspNetCore.Mvc;
using Sispae.Entities.MRecursos;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Controllers
{
    public class RecursosController : Controller
    {
        private readonly IRepositorioRecursos vRecursos;
        private readonly IRepositorioPerfiles vPerfil;

        public RecursosController(IRepositorioRecursos iRecursos, IRepositorioPerfiles iPerfil)
        {
            this.vRecursos = iRecursos ?? throw new ArgumentNullException(nameof(iRecursos));
            this.vPerfil = iPerfil ?? throw new ArgumentNullException(nameof(iPerfil));
        }

        [HttpPost]
        [Route("/recursos/insertaRecursos")]
        public async Task<IActionResult> InsertarRecursos([FromForm] Recursos recursos)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            if (success == 1)
            {
                int integra = await vRecursos.insertaRecurso(recursos); //obtenemos el proyecto a actualizar
                if (integra != -1)
                {
                    return Ok(integra);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

        [HttpPost]
        [Route("/recursos/actualizaRecursos")]
        public async Task<IActionResult> ActualizarRecursos([FromForm] Recursos recursos)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            if (success == 1)
            {
                int integra = await vRecursos.actualizaRecurso(recursos); //obtenemos el proyecto a actualizar
                if (integra != -1 && integra != 0)
                {
                    return Ok(integra);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

        [HttpPost]
        [Route("/recursos/eliminarRecursos")]
        public async Task<IActionResult> EliminarRecursos([FromBody] Recursos recursos)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            if (success == 1)
            {
                int integra = await vRecursos.eliminaRecurso(recursos); //obtenemos el proyecto a actualizar
                if (integra != -1 && integra != 0)
                {
                    return Ok(integra);
                }
                return BadRequest();
            }
            return Redirect("/error/denied");
        }

        [HttpGet]
        [Route("/recursos/validaRecursos/{seguimiento}")]
        public async Task<IActionResult> RecursosSinOficio(int seguimiento)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "adjudicar proyecto");
            if (success == 1)
            {
                int integra = await vRecursos.RecursosSinOficio(seguimiento); //obtenemos el proyecto a actualizar
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
