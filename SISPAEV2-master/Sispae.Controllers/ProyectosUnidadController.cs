using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sispae.Entities.MProyectos;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Controllers
{
    [Authorize]
    public class ProyectosUnidadController:Controller
    {
        private readonly IRepositorioProyectosUnidad vPunidad;
        private readonly IRepositorioPerfiles vPerfil;

        public ProyectosUnidadController(IRepositorioProyectosUnidad iPunidad, IRepositorioPerfiles iPerfil)
        {
            this.vPunidad = iPunidad ?? throw new ArgumentNullException(nameof(iPunidad));
            this.vPerfil = iPerfil ?? throw new ArgumentNullException(nameof(iPerfil));
        }

        [HttpPost]
        [Route("/proyectosUnidad/insertaPU")]
        public async Task<ActionResult> insertaUegProyecto([FromBody] List<ProyectosUeg> unidad)
        {
            int success = 0;
            success = await vPunidad.insertaUEGProyecto(unidad);
            if (success != -1)
            {
                return Ok(success);
            }
            return BadRequest();
        }

        [Route("/proyectosUnidad/eliminaPU/{proyecto}/{unidad}/{ejercicio}")]
        public async Task<ActionResult> eliminaUegProyecto(int proyecto, int unidad, int ejercicio)
        {
            int success = 0;
            success = await vPunidad.eliminaUEGProyecto(proyecto,unidad,ejercicio);
            if (success != -1)
            {
                return Ok(success);
            }
            return BadRequest();
        }
    }
}
