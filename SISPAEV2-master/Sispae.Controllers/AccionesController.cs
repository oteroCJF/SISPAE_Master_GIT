using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sispae.Controllers
{
   public class AccionesController : Controller
    {
        private readonly IRepositorioEntregables vEntregables;
        private readonly IHostingEnvironment environment;

        public AccionesController(IRepositorioEntregables iEntregables, IHostingEnvironment environment)
        {
            this.vEntregables = iEntregables ?? throw new ArgumentNullException(nameof(iEntregables));
            this.environment = environment;
        }

        [HttpGet]
        [Route("/view/oficio/{folderOficio}/{oficio}")]
        public IActionResult verAcuseFinancieros(string folderOficio, string oficio)
        {
            string folderName = Directory.GetCurrentDirectory() + "\\Oficios\\" + folderOficio;
            string webRootPath = environment.ContentRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string pathArchivo = Path.Combine(newPath, oficio);

            if (System.IO.File.Exists(pathArchivo))
            {
                Stream stream = System.IO.File.Open(pathArchivo, FileMode.Open);

                return File(stream, "application/pdf");
            }
            return NotFound();
        }

        [HttpGet]
        [Route("/view/entregable/{seguimiento}/{file}")]
        public IActionResult verEntregablesSeguimiento(string seguimiento, string file)
        {
            string folderName = Directory.GetCurrentDirectory() + "\\Entregables\\" + seguimiento;
            string webRootPath = environment.ContentRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string pathArchivo = Path.Combine(newPath, file);

            if (System.IO.File.Exists(pathArchivo))
            {
                Stream stream = System.IO.File.Open(pathArchivo, FileMode.Open);

                return File(stream, "application/pdf");
            }
            return NotFound();
        }
    }
}
