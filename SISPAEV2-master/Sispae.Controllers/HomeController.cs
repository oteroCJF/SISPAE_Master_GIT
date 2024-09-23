using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceReference1;
using Sispae.Entities.MDashboards;
using Sispae.Entities.MLogin;
using Sispae.Entities.Vistas;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SISPAEV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositorioLogin vLogin;
        private readonly IRepositorioDashboards vDashboard;
        private readonly IRepositorioUsuarios vUsuarios;

        public HomeController(IRepositorioLogin iLogin, IRepositorioUsuarios iUsuarios, IRepositorioDashboards iDashboard)
        {
            this.vLogin = iLogin ?? throw new ArgumentNullException(nameof(iLogin));
            this.vDashboard = iDashboard ?? throw new ArgumentNullException(nameof(iDashboard));
            this.vUsuarios = iUsuarios ?? throw new ArgumentNullException(nameof(iUsuarios));
        }

        [Authorize]
        public async Task<IActionResult> Index([FromQuery(Name = "Anio")] int anio, [FromQuery(Name = "Tipo")] string tipo, 
            [FromQuery(Name = "Estatus")] string estatus)
        {
            DModels dModels = new DModels();
            if (!User.Claims.ElementAt(5).Value.Equals("[]"))
            {
                dModels.Anio = anio;
                if (anio != 0)
                {
                    dModels.dashboardEstatus = await vDashboard.GetDashboardEstatus(anio);
                    dModels.dashboardEstatusA = await vDashboard.GetDashboardEstatusAhorros(anio);
                    if ((tipo != null && !tipo.Equals("")) && (estatus!= null && !estatus.Equals("")))
                    {
                        dModels.Tipo = tipo;
                        dModels.Estatus = estatus;
                        dModels.dashboardpueg = await vDashboard.GetDashboardProyectosUEG(anio,tipo, estatus);
                    }
                }
                return View(dModels);
            }
            else
            {
                return Redirect("/error/perfilNotFound");
            }
        }

        [HttpGet]
        [Route("/home/download/casesg2")]
        public IActionResult getPlantilla()
        {
            string fileName = @"e:\Plantillas CASESGV2\DocsV2\Guia\Guia rápida del sistema CASESG_2.0.pdf";
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
            return File(fileBytes, "application/pdf", "Guia_CASESGV2.pdf");
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Home");
            }
            return View();
        }

        [HttpPost("login")]
        public async Task<ActionResult<DatosUsuario>> Login(string username, string password, string returnUrl)
        {
            //Validamos a traves del Web Service los datos de Login
            var client = new ServicioSeguridadClient();
            ValidaUsuarioPorSistemaCompletoResponse validacion = await client.ValidaUsuarioPorSistemaCompletoAsync(277, username, password, "");
            TServicioValidacion validacionRespuesta = validacion.ValidaUsuarioPorSistemaCompletoResult;

            DatosUsuario dtUser = new DatosUsuario();
            if (validacionRespuesta.EsValido)
            {
                int existUser = await vLogin.buscaUsuario(username, password);
                if (existUser != -1)
                {

                    List<VModulosUsuario> modulos = null;
                    dtUser = await vLogin.login(username, password);
                    var claims = new List<Claim>();

                    modulos = await vLogin.getModulosByUser((int)dtUser.Id);

                    claims.Add(new Claim(ClaimTypes.NameIdentifier, dtUser.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, dtUser.Empleado));
                    claims.Add(new Claim(ClaimTypes.Role, dtUser.Perfiles));//contiene si es o no admin
                    claims.Add(new Claim(ClaimTypes.SerialNumber, dtUser.Expediente.ToString()));
                    claims.Add(new Claim(ClaimTypes.Sid, dtUser.ClaveInmueble.ToString()));
                    claims.Add(new Claim("Modulos", JsonConvert.SerializeObject(modulos)));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);

                    return Redirect(returnUrl != null ? returnUrl : "/Home");
                }
                else
                {
                    int insertUser = await vUsuarios.insertaUsuario(validacionRespuesta.DatosUsuario.Nodes[1].ToString(), password);
                    if (insertUser != 0)
                    {
                        List<VModulosUsuario> modulos = null;
                        dtUser = await vLogin.login(username, password);
                        var claims = new List<Claim>();
                        modulos = await vLogin.getModulosByUser((int)dtUser.Id);
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, dtUser.Id.ToString()));
                        claims.Add(new Claim(ClaimTypes.Name, dtUser.Empleado));
                        claims.Add(new Claim(ClaimTypes.Role, dtUser.Perfiles));//contiene si es o no admin
                        claims.Add(new Claim(ClaimTypes.SerialNumber, dtUser.Expediente.ToString()));
                        claims.Add(new Claim(ClaimTypes.Sid, dtUser.ClaveInmueble.ToString()));
                        claims.Add(new Claim("Modulos", JsonConvert.SerializeObject(modulos)));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);

                        return Redirect(returnUrl != null ? returnUrl : "/Home");
                    }
                }
            }
            TempData["Error"] = "El usuario/contraseña no es correcto.";
            return View("login");
        }

        [Authorize]
        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Login");
        }

        [HttpGet]
        [Route("/Account/AccessDenied")]
        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}
