using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sispae.Entities.MHistorial;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Controllers
{
    [Authorize]
    public class HistorialesController : Controller
    {
        private readonly IRepositorioHistoriales vHistorial;
        private readonly IRepositorioUsuarios vUsuario;
        private readonly IRepositorioPerfiles vPerfil;

        public HistorialesController(IRepositorioHistoriales iHistorial, IRepositorioUsuarios iUsuario, IRepositorioPerfiles iPerfil)
        {
            this.vHistorial = iHistorial ?? throw new ArgumentNullException(nameof(iHistorial));
            this.vUsuario = iUsuario ?? throw new ArgumentNullException(nameof(iUsuario));
            this.vPerfil = iPerfil ?? throw new ArgumentNullException(nameof(iPerfil));
        }

        [HttpPost]
        [Route("/historial/nuevoHistorialProyecto")]
        public async Task<IActionResult> NuevoHistorialProyecto([FromBody] HistorialProyectos historial)
        {
                int insert = await vHistorial.capturaHistorial(historial); //obtenemos el proyecto a actualizar
                if (insert != -1)
                {
                    return Ok(insert);
                }
                return BadRequest();
        }

        private int UserId()
        {
            return Convert.ToInt32(User.Claims.ElementAt(0).Value);
        }


    }
}
