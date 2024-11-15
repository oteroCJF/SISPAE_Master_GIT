using Microsoft.AspNetCore.Mvc;
using Sispae.Entities.MEntregables;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Controllers
{
    public class EntregablesController : Controller
    {
        private readonly IRepositorioEntregables vEntregables;
        private readonly IRepositorioSeguimiento vSeguimiento;
        private readonly IRepositorioPerfiles vPerfiles;        

        public EntregablesController(IRepositorioEntregables iEntregables, IRepositorioSeguimiento iSeguimiento, IRepositorioPerfiles ivPerfiles)
        {
            this.vEntregables = iEntregables ?? throw new ArgumentNullException(nameof(iEntregables));
            this.vSeguimiento = iSeguimiento ?? throw new ArgumentNullException(nameof(iSeguimiento));
            this.vPerfiles = ivPerfiles ?? throw new ArgumentNullException(nameof(ivPerfiles));
        }


        [Route("/entregables/getEntregablesBySeguimiento/{seguimiento}")]
        [HttpPost]
        public async Task<IActionResult> getEntregablesBySeguimiento(int seguimiento)
        {
            int success = await vPerfiles.getPermiso(UserId(), modulo(), "ver");
            if (success == 1)
            {
                List<Entregables> entregables = await vEntregables.getEntregables(seguimiento);
                if (entregables != null)
                {
                    return Ok(entregables);
                }
            }
            return BadRequest(); ;
        }

        [Route("/entregables/getEntregablesMemorias/{integracion}")]
        [HttpPost]
        public async Task<IActionResult> getEntregablesMemorias(int seguimiento)
        {
            int success = await vPerfiles.getPermiso(UserId(), modulo(), "ver");
            if (success == 1)
            {
                List<Entregables> entregables = await vEntregables.getEntregablesMemorias(seguimiento);
                if (entregables != null)
                {
                    return Ok(entregables);
                }
            }
            return BadRequest(); ;
        }

        [Route("/entregables/insertaEntregable")]
        [HttpPost]
        public async Task<IActionResult> insertaEntregables([FromForm] Entregables entregable)
        {
            int success = await vPerfiles.getPermiso(UserId(), modulo(), "crear");
            if (success == 1)
            {
                int insert = await vEntregables.insertaEntregable(entregable);
                if (insert != -1 && insert != 0)
                {
                    return Ok(insert);
                }
            }
            return BadRequest(); ;
        }

        [Route("/entregables/insertaMemoria")]
        [HttpPost]
        public async Task<IActionResult> insertaMemorias([FromForm] Entregables entregable)
        {
            int success = await vPerfiles.getPermiso(UserId(), modulo(), "crear");
            if (success == 1)
            {
                int insert = await vEntregables.insertaEntregableMemoria(entregable);
                if (insert != -1 && insert != 0)
                {
                    return Ok(insert);
                }
            }
            return BadRequest(); ;
        }

        [Route("/entregables/actualizaEntregable")]
        [HttpPost]
        public async Task<IActionResult> actualizaEntregables([FromForm] Entregables entregable)
        {
            int success = await vPerfiles.getPermiso(UserId(), modulo(), "crear");
            if (success == 1)
            {
                int insert = await vEntregables.actualizaEntregable(entregable);
                if (insert != -1 && insert != 0)
                {
                    return Ok(insert);
                }
            }
            return BadRequest();
        }

        [Route("/entregables/eliminaEntregable")]
        [HttpPost]
        public async Task<IActionResult> eliminaEntregables([FromBody] Entregables entregable)
        {
            int success = await vPerfiles.getPermiso(UserId(), modulo(), "eliminar");
            if (success == 1)
            {
                int insert = await vEntregables.eliminaEntregables(entregable);
                if (insert != -1 && insert != 0)
                {
                    return Ok(insert);
                }
            }
            return BadRequest();
        }

        [Route("/entregables/eliminaEntregableMemoria")]
        [HttpPost]
        public async Task<IActionResult> eliminaEntregableMemoria([FromBody] Entregables entregable)
        {
            int success = await vPerfiles.getPermiso(UserId(), modulo(), "eliminarMemoria");
            if (success == 1)
            {
                int insert = await vEntregables.eliminaEntregableMemoria(entregable);
                if (insert != -1 && insert != 0)
                {
                    return Ok(insert);
                }
            }
            return BadRequest();
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
