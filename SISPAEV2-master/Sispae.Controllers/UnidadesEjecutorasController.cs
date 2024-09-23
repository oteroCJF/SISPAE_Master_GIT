using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sispae.Entities.MUEGS;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Controllers
{
    [Authorize]
    public class UnidadesEjecutorasController: Controller
    {
        private readonly IRepositorioUnidadesEjecutoras vUnidad;
        private readonly IRepositorioPerfiles vPerfil;

        public UnidadesEjecutorasController(IRepositorioUnidadesEjecutoras iVinmuebles, IRepositorioPerfiles iPerfil)
        {
            this.vUnidad = iVinmuebles ?? throw new ArgumentNullException(nameof(iVinmuebles));
            this.vPerfil = iPerfil ?? throw new ArgumentNullException(nameof(iPerfil));
        }

        //Listado de todos las Administraciones
        [Route("/ueg/index")]
        public async Task<IActionResult> Index(List<UEG> ueg)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "ver");
            if (success == 1)
            {
                ueg = await vUnidad.GetUnidadesEjecutoras();
                return View(ueg);
            }
            return Redirect("/error/denied");
        }

        [Route("/ueg/unidades")]
        public async Task<IActionResult> GetUEGS()
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "ver");
            if (success == 1)
            {
                List<UEG> ueg = await vUnidad.GetUnidadesEjecutoras();
                return Ok(ueg);
            }
            return BadRequest();
        }

        [Route("/ueg/actualiza")]
        public async Task<IActionResult> UpdateUnidadEjecutora([FromBody] UEG ueg)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "actualizar");
            if (success == 1)
            {
                int update = await vUnidad.UpdateUnidadEjecutora(ueg);
                return Ok(update);
            }
            return Redirect("/error/denied");
        }
        
        [Route("/ueg/delete/{id?}")]
        public async Task<IActionResult> DeleteUnidadEjecutora(int id)
        {
            int success = await vPerfil.getPermiso(UserId(), modulo(), "eliminar");
            if (success == 1)
            {
                int delete= await vUnidad.DeleteUnidadEjecutora(id);
                return Ok(delete);
            }
            return Redirect("/error/denied");
        }
        private int UserId()
        {
            return Convert.ToInt32(User.Claims.ElementAt(0).Value);
        }

        private string modulo()
        {
            return "UEG";
        }
    }
}
