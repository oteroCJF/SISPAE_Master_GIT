using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sispae.Entities.MPartidasPresupuestales;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Controllers
{
    [Authorize]
    public class PartidasPresupuestalesController : Controller
    {
        private IRepositorioPartidasPresupuestales vPartidas;
        private readonly IRepositorioPerfiles vPerfil;

        public PartidasPresupuestalesController(IRepositorioPartidasPresupuestales iPartidas, IRepositorioPerfiles iPerfil)
        {
            this.vPartidas = iPartidas ?? throw new ArgumentNullException(nameof(iPartidas));
            this.vPerfil = iPerfil ?? throw new ArgumentNullException(nameof(iPerfil));
        }

        [HttpGet]
        [Route("/partidas/getPartidas/{integracion}")]
        public async Task<IActionResult> GetUEGS(int integracion)
        {
            List<PartidasPresupuestales> partidas = await vPartidas.GetPartidasPresupuestales(integracion);
            if (partidas != null)
            {
                return Ok(partidas);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("/partidas/getPartidasProyecto/{id?}")]
        public async Task<IActionResult> GetPartidasProyecto(int id)
        {
            List<PartidasPresupuestales> partidas = await vPartidas.GetPartidasProyecto(id);
            if (partidas != null)
            {
                return Ok(GetTablePartidasPresupuestales(partidas));
            }
            return BadRequest();
        }
        
        [HttpGet]
        [Route("/partidas/getPartidasIntegracion/{id?}")]
        public async Task<IActionResult> GetPartidasIntegracion(int id)
        {
            List<PartidasPresupuestales> partidas = await vPartidas.GetPartidasProyecto(id);
            if (partidas != null)
            {
                return Ok(partidas.Sum(p => p.Monto));
            }
            return BadRequest();
        }
        
        public string GetTablePartidasPresupuestales(List<PartidasPresupuestales> partidas)
        {
            string table = "";
            var i = 0;
            foreach (var pr in partidas)
            {
                i++;
                table +=
                    "<tr>" +
                       "<td class='text-center'>" + i + "</td>" +
                       "<td class='text-center'>" + pr.Nombre + "</td>" +
                       "<td class='text-center'>" + String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C}", pr.Monto) + "</td>" +
                       "<td class='text-center'><a href='#' class='delete_partida text-danger text-center' data-partida='" + pr.Id + "' data-nombre='" + pr.Nombre + "'><i class='far fa-times'></i></a></td>" +
                    "</tr>";
            }
            return table;
        }
        
        [HttpPost]
        [Route("/partidas/insertaPartidaProyecto")]
        public async Task<IActionResult> insertaPartidaProyecto([FromBody] PartidasProyecto partidasProyecto)
        {
            int insert = await vPartidas.insertaPartidasByProyecto(partidasProyecto);
            if (insert != 0)
            {
                return Ok(insert);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("/partidas/eliminaPartidasProyecto/{integracion?}/{partida?}")]
        public async Task<IActionResult> EliminaPartidasProyecto(int integracion, int partida)
        {
            int delete = await vPartidas.DeletePartidasProyecto(integracion, partida);
            if (delete != -1)
            {
                return Ok(delete);
            }
            return BadRequest();
        }

    }
}
