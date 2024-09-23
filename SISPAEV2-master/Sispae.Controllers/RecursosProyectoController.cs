using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sispae.Entities.MProyectos;
using Sispae.Entities.MRecursosProyecto;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Controllers
{
    [Authorize]
    public class RecursosProyectoController : Controller
    {
        private readonly IRepositorioRecursosProyecto _rProyecto;
        private readonly IRepositorioPerfiles vPerfil;

        public RecursosProyectoController(IRepositorioRecursosProyecto rProyecto, IRepositorioPerfiles iPerfil)
        {
            _rProyecto = rProyecto ?? throw new ArgumentNullException(nameof(rProyecto));
            this.vPerfil = iPerfil ?? throw new ArgumentNullException(nameof(iPerfil));
        }

        [HttpGet]
        [Route("/recursosP/getRecursosP/{integracion}")]
        public async Task<ActionResult> GetRecursosProyecto(int integracion)
        {
            List<VRecursosProyecto> success = await _rProyecto.GetRecursosProyecto(integracion);
            if (success != null)
            {
                return Ok(success);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("/recursosP/insertaRP")]
        public async Task<ActionResult> insertaUegProyecto([FromBody] RecursosProyectos rProyecto)
         {
            int success = 0;
            success = await _rProyecto.insertaRecursosProyecto(rProyecto);
            if (success != -1)
            {
                return Ok(success);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("/recursosP/eliminaRP/{integracion}/{mes}/{monto}")]
        public async Task<ActionResult> eliminaUegProyecto(int integracion, int mes, decimal monto)
        {
            int success = 0;
            success = await _rProyecto.eliminaRecursoProyecto(integracion, mes, monto);
            if (success != -1)
            {
                return Ok(success);
            }
            return BadRequest();
        }
    }
}
