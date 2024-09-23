using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using Sispae.Entities.MProyectos;
using Sispae.Entities.MReportes;
using Sispae.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sispae.Controllers
{
    public class ReportesController : Controller
    {
        private string[] capitulos = { "2000", "3000","5000" };


        private readonly IWebHostEnvironment web;
        private readonly IRepositorioProyectos vProyectos;
        private readonly IRepositorioReportesUEG vReportes;
        private readonly IRepositorioUnidadesEjecutoras vUnidad;

        public ReportesController(IWebHostEnvironment vweb, IRepositorioReportesUEG iReportes, IRepositorioUnidadesEjecutoras iUnidad, IRepositorioProyectos iProyectos)
        {
            this.web = vweb;
            this.vProyectos = iProyectos ?? throw new ArgumentNullException(nameof(iProyectos));
            this.vReportes = iReportes;
            this.vUnidad = iUnidad;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        [Route("/reportes/index")]
        public async Task<IActionResult> Index([FromQuery(Name = "Anio")] int anio, [FromQuery(Name = "Catalogo")] string catalogo,
            [FromQuery(Name = "Reporte")] string reporte,[FromQuery(Name = "Presupuesto")] string presupuesto, [FromQuery(Name = "UEG")] int UEG, 
            [FromQuery(Name = "Ordenar")] bool ordenar)
        {
            Models models = new Models();
            models.Anio = anio;
            models.ueg = UEG;
            models.Catalogo = catalogo;
            models.Presupuesto = presupuesto;
            models.Reporte = reporte;
            models.Ordenar = ordenar;
            if (anio != 0 && !catalogo.Equals("") && (reporte != null && (reporte.Equals("UEG") || reporte.Equals("Punto"))))
            {
                models.uegsAsignadas = await vUnidad.GetUnidadesEjecutorasByUser(UserId());
            }

            if (ordenar)
            {
                models.lProyectos = await vProyectos.GetProyectosEstatusByUEG(UEG, "Todos", anio, UserId(), catalogo);
            }

            return View(models);
        }

        [Route("/reportes/reporteGeneral/{ejercicio}/{tipo}")]
        public async Task<IActionResult> GetReporteGeneral(int ejercicio, string tipo)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteGeneral.rdlc";
            local.ReportPath = path;
            local.SetParameters(new[] { new ReportParameter("unidad", "Reporte General del Programa Anual de Ejecución") });
            var proyectos = await vReportes.GetReporteGeneralUEG(ejercicio, tipo);
            local.DataSources.Add(new ReportDataSource("ProyectosGeneral", proyectos));
            var pdf = local.Render("PDF");
            return File(pdf, "application/pdf");
        }

        [Route("/reportes/reporteGeneral/word/{ejercicio}/{tipo}")]
        public async Task<IActionResult> GetReporteGeneralWord(int ejercicio, string tipo)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteGeneral.rdlc";
            local.ReportPath = path;
            local.SetParameters(new[] { new ReportParameter("unidad", "Reporte General del Programa Anual de Ejecución") });
            var proyectos = await vReportes.GetReporteGeneralUEG(ejercicio, tipo);
            local.DataSources.Add(new ReportDataSource("ProyectosGeneral", proyectos));
            var word = local.Render("WORDOPENXML");
            return File(word, "application/msWORD", "ReporteGeneral_" + DateTime.Now + ".docx");
        }

        [Route("/reportes/calendarizacion")]
        public async Task<IActionResult> GetCalendarizacion(int ejercicio, string tipo)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteCalendarizacion.rdlc";
            local.ReportPath = path;
            var proyectos = await vReportes.GetCalendarizacion();
            local.DataSources.Add(new ReportDataSource("Calendarizacion", proyectos));
            var pdf = local.Render("PDF");
            return File(pdf, "application/pdf");
        }
        
        [Route("/reportes/calendarizacion/word")]
        public async Task<IActionResult> GetCalendarizacionWord(int ejercicio, string tipo)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteCalendarizacion.rdlc";
            local.ReportPath = path;
            var proyectos = await vReportes.GetCalendarizacion();
            local.DataSources.Add(new ReportDataSource("Calendarizacion", proyectos));
            var word = local.Render("WORDOPENXML");
            return File(word, "application/msWORD", "ReporteCalendarizacion_" + DateTime.Now + ".docx");
        }

        [Route("/reportes/reporteGeneralTP/{ejercicio}")]
        public async Task<IActionResult> GetReporteGeneralTP(int ejercicio, string tipo)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteGeneralTP.rdlc";
            local.ReportPath = path;
            local.SetParameters(new[] { new ReportParameter("unidad", "Reporte General del Programa Anual de Ejecución") });
            var proyectos = await vReportes.GetReporteGeneralTP(ejercicio);
            local.DataSources.Add(new ReportDataSource("ProyectosGeneral", proyectos));
            var pdf = local.Render("PDF");
            return File(pdf, "application/pdf");
        }

        [Route("/reportes/reporteGeneralTP/word/{ejercicio}")]
        public async Task<IActionResult> GetReporteGeneralTPWord(int ejercicio)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteGeneralTP.rdlc";
            local.ReportPath = path;
            local.SetParameters(new[] { new ReportParameter("unidad", "Reporte General del Programa Anual de Ejecución") });
            var proyectos = await vReportes.GetReporteGeneralTP(ejercicio);
            local.DataSources.Add(new ReportDataSource("ProyectosGeneral", proyectos));
            var word = local.Render("WORDOPENXML");
            return File(word, "application/msWORD", "ReporteGeneral_" + DateTime.Now + ".docx");
        }

        [Route("/reportes/reporteGeneralAhorros/{ejercicio}/{tipo}")]
        public async Task<IActionResult> GetReporteAhorrosGeneral(int ejercicio, string tipo)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteGeneralAhorros.rdlc";
            local.ReportPath = path;
            local.SetParameters(new[] { new ReportParameter("unidad", "Reporte General del Programa Anual de Ejecución") });
            var proyectos = await vReportes.GetReporteGeneralAhorrosUEG(ejercicio, tipo);
            local.DataSources.Add(new ReportDataSource("ProyectosGeneral", proyectos));
            var pdf = local.Render("PDF");
            return File(pdf, "application/pdf");
        }

        [Route("/reportes/reporteGeneralAhorros/word/{ejercicio}/{tipo}")]
        public async Task<IActionResult> GetReporteGeneralAhorrosWord(int ejercicio, string tipo)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteGeneralAhorros.rdlc";
            local.ReportPath = path;
            local.SetParameters(new[] { new ReportParameter("unidad", "Reporte General del Programa Anual de Ejecución") });
            var proyectos = await vReportes.GetReporteGeneralAhorrosUEG(ejercicio, tipo);
            local.DataSources.Add(new ReportDataSource("ProyectosGeneral", proyectos));
            var word = local.Render("WORDOPENXML");
            return File(word, "application/msWORD", "ReporteGeneral_" + DateTime.Now + ".docx");
        }

        [Route("/reportes/ueg/{ueg}/{ejercicio}/{tipo}")]
        public async Task<IActionResult> GetReporteUEG(int ueg,int ejercicio,string tipo)
        {
            LocalReport local = new LocalReport();
            var path = "";
            if (tipo.Equals("Ahorros")) {
                path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteUEGAhorros.rdlc";
            }
            else
            {
                path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteUEG.rdlc";
            }
            local.ReportPath = path;
            var unidad = await vUnidad.GetUnidadEjecutoraById(ueg);
            local.SetParameters(new[] { new ReportParameter("unidad", unidad.NumeroUEG +":" + unidad.Descripcion) });
            List<ReporteUEG> proyectos = new List<ReporteUEG>();
            for (int c = 0, m = 0; c < capitulos.Length; c++)
            {
                if (proyectos.Count == 0)
                {
                    m = 0;
                }
                else
                {
                    m = proyectos[proyectos.Count - 1].Id;
                }
                proyectos = await vReportes.GetProyectosUEG(ueg, ejercicio, Convert.ToInt32(capitulos[c]), tipo, m);
                local.DataSources.Add(new ReportDataSource("Proyectos" + capitulos[c], proyectos));
            }
            local.SetParameters(new[] { new ReportParameter("presupuesto", tipo) });
            var pdf = local.Render("PDF");
            return File(pdf, "application/pdf");
        }

        [Route("/reportes/ueg/word/{ueg}/{ejercicio}/{tipo}")]
        public async Task<IActionResult> GetReporteUEGWord(int ueg, int ejercicio,string tipo)
        {
            LocalReport local = new LocalReport();
            var path = "";
            if (tipo.Equals("Ahorros"))
            {
                path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteUEGAhorros.rdlc";
            }
            else
            {
                path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteUEG.rdlc";
            }
            local.ReportPath = path;
            var unidad = await vUnidad.GetUnidadEjecutoraById(ueg);
            local.SetParameters(new[] { new ReportParameter("unidad", unidad.NumeroUEG + ":" + unidad.Descripcion) });
            var proyectos = new List<ReporteUEG>();
            for (int c = 0, m = 0; c < capitulos.Length; c++)
            {
                m = c;
                if (proyectos.Count == 0)
                {
                    m = 0;
                }
                proyectos = await vReportes.GetProyectosUEG(ueg, ejercicio, Convert.ToInt32(capitulos[c]), tipo,m);
                local.DataSources.Add(new ReportDataSource("Proyectos" + capitulos[c], proyectos));
            }
            local.SetParameters(new[] { new ReportParameter("presupuesto", tipo) });
            var word= local.Render("WORDOPENXML");
            return File(word, "application/msWORD", "Reporte"+unidad.NumeroUEG+"_"+ DateTime.Now + ".docx");
        }

        [Route("/reporte/general/ueg/{ueg}/{ejercicio}/{tipo}")]
        public async Task<IActionResult> GetReporteGeneralUEG(int ueg, int ejercicio, string tipo)
        {
            LocalReport local = new LocalReport();
            var path = "";
            if (tipo.Equals("Ahorros"))
            {
                path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteUEGAhorros.rdlc";
            }
            else
            {
                path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteGeneralUEG.rdlc";
            }
            local.ReportPath = path;
            var unidad = await vUnidad.GetUnidadEjecutoraById(ueg);
            local.SetParameters(new[] { new ReportParameter("unidad", unidad.NumeroUEG + ":" + unidad.Descripcion) });
            var proyectos = new List<ReporteUEG>();
            proyectos = await vReportes.GetProyectosByUEG(ueg, ejercicio, tipo);
            local.DataSources.Add(new ReportDataSource("ProyectosUEG", proyectos));
            local.SetParameters(new[] { new ReportParameter("presupuesto", tipo) });
            var pdf = local.Render("PDF");
            return File(pdf, "application/pdf");
        }

        [Route("/reporte/general/ueg/word/{ueg}/{ejercicio}/{tipo}")]
        public async Task<IActionResult> GetReporteGenetalUEGWord(int ueg, int ejercicio,string tipo)
        {
            LocalReport local = new LocalReport();
            var path = "";
            if (tipo.Equals("Ahorros"))
            {
                path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteUEGAhorros.rdlc";
            }
            else
            {
                path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteGeneralUEG.rdlc";
            }
            local.ReportPath = path;
            var unidad = await vUnidad.GetUnidadEjecutoraById(ueg);
            local.SetParameters(new[] { new ReportParameter("unidad", unidad.NumeroUEG + ":" + unidad.Descripcion) });
            var proyectos = new List<ReporteUEG>();
            proyectos = await vReportes.GetProyectosByUEG(ueg, ejercicio, tipo);
            local.DataSources.Add(new ReportDataSource("ProyectosUEG", proyectos));
            local.SetParameters(new[] { new ReportParameter("presupuesto", tipo) });
            var word= local.Render("WORDOPENXML");
            return File(word, "application/msWORD", "Reporte"+unidad.NumeroUEG+"_"+ DateTime.Now + ".docx");
        }

        [Route("/reportes/reportePunto/{ejercicio}")]
        public async Task<IActionResult> GetReportePunto(int ejercicio)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReportePunto.rdlc";
            local.ReportPath = path;
            var proyectos = await vReportes.GetReportePunto(ejercicio);
            local.DataSources.Add(new ReportDataSource("ReportePunto", proyectos));
            var excel= local.Render("EXCELOPENXML");
            return File(excel, "application/msexcel", "ReportePunto_" + DateTime.Now + ".xlsx");
        }

        [Route("/reportes/reportePuntoAhorros/{ejercicio}")]
        public async Task<IActionResult> GetReportePuntoAhorros(int ejercicio)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteAhorros.rdlc";
            local.ReportPath = path;
            var proyectos = await vReportes.GetReportePuntoAhorros(ejercicio);
            local.DataSources.Add(new ReportDataSource("ReportePuntoAhorros", proyectos));
            var excel = local.Render("EXCELOPENXML");
            return File(excel, "application/msexcel", "ReportePunto_" + DateTime.Now + ".xlsx");
        }

        [Route("/reportes/reportePunto/{ejercicio}/{ueg}")]
        public async Task<IActionResult> GetReportePuntoByUEG(int ejercicio, int ueg)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReportePunto.rdlc";
            local.ReportPath = path;
            var proyectos = await vReportes.GetReportePuntoByUEG(ejercicio,ueg);
            local.DataSources.Add(new ReportDataSource("ReportePunto", proyectos));
            var excel = local.Render("EXCELOPENXML");
            return File(excel, "application/msexcel", "ReportePunto_" + DateTime.Now + ".xlsx");
        }

        [Route("/reportes/reportePuntoAhorros/{ejercicio}/{ueg}")]
        public async Task<IActionResult> GetReportePuntoAhorrosByUEG(int ejercicio, int ueg)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteAhorros.rdlc";
            local.ReportPath = path;
            var proyectos = await vReportes.GetReportePuntoAhorrosByUEG(ejercicio, ueg);
            local.DataSources.Add(new ReportDataSource("ReportePuntoAhorros", proyectos));
            var excel = local.Render("EXCELOPENXML");
            return File(excel, "application/msexcel", "ReportePunto_" + DateTime.Now + ".xlsx");
        }
        
        [Route("/reportes/reporteAnticipadas/{ejercicio}/{ueg}")]
        public async Task<IActionResult> GetReporteAnticipadas(int ejercicio, int ueg)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteAnticipadas.rdlc";
            local.ReportPath = path;
            var proyectos = new List<ReporteAnticipadas>();
            for (int c = 0, m = 0; c < capitulos.Length; c++)
            {
                if (!capitulos[c].Equals("5000")) {
                    if (proyectos.Count == 0)
                    {
                        m = 0;
                    }
                    else
                    {
                        m = proyectos[proyectos.Count - 1].ClaveProyecto;
                    }
                    proyectos = await vReportes.GetReporteAnticipadas(ejercicio, ueg, Convert.ToInt32(capitulos[c]),m);
                    local.DataSources.Add(new ReportDataSource("ReporteAnticipadas" + capitulos[c], proyectos));
                }
            }
            var unidad = await vUnidad.GetUnidadEjecutoraById(ueg);
            local.SetParameters(new[] { new ReportParameter("unidad", unidad.NumeroUEG+" "+unidad.Descripcion) });
            var pdf = local.Render("PDF");
            return File(pdf, "application/pdf");
        }
        
        [Route("/reportes/excel/reporteAnticipadas/{ejercicio}/{ueg}")]
        public async Task<IActionResult> GetReporteAnticipadasExcel(int ejercicio, int ueg)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteAnticipadas.rdlc";
            local.ReportPath = path;
            var proyectos = new List<ReporteAnticipadas>();
            for (int c = 0, m = 0; c < capitulos.Length; c++)
            {
                if (!capitulos[c].Equals("5000")) {
                    if (proyectos.Count == 0)
                    {
                        m = 0;
                    }
                    else
                    {
                        m = proyectos[proyectos.Count - 1].ClaveProyecto;
                    }
                    proyectos = await vReportes.GetReporteAnticipadas(ejercicio, ueg, Convert.ToInt32(capitulos[c]),m);
                    local.DataSources.Add(new ReportDataSource("ReporteAnticipadas" + capitulos[c], proyectos));
                }
            }
            var unidad = await vUnidad.GetUnidadEjecutoraById(ueg);
            local.SetParameters(new[] { new ReportParameter("unidad", unidad.NumeroUEG+" "+unidad.Descripcion) });
            var excel = local.Render("EXCELOPENXML");
            return File(excel, "application/msexcel", "ReporteAnticipadas" + DateTime.Now + ".xlsx");
        }

        [Route("/reportes/calendario/{ejercicio}/{ueg}")]
        public async Task<IActionResult> GetReporteCalendarioUEG(int ejercicio, int ueg)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteCalendarioUEG.rdlc";
            local.ReportPath = path;
            var proyectos = await vReportes.GetReporteCalendarioUEG(ejercicio, ueg);
            local.DataSources.Add(new ReportDataSource("Calendarizacion", proyectos));

            var pdf = local.Render("PDF");
            return File(pdf, "application/pdf");
        }
        
        [Route("/reportes/calendario/excel/{ejercicio}/{ueg}")]
        public async Task<IActionResult> GetReporteCalendarioUEGExcel(int ejercicio, int ueg)
        {
            LocalReport local = new LocalReport();
            var path = Directory.GetCurrentDirectory() + "\\Reportes\\ReporteCalendarioUEG.rdlc";
            local.ReportPath = path;
            var proyectos = await vReportes.GetReporteCalendarioUEG(ejercicio, ueg);
            local.DataSources.Add(new ReportDataSource("Calendarizacion", proyectos));

            var excel = local.Render("EXCELOPENXML");
            return File(excel, "application/msexcel", "Reporte_Calendario" + DateTime.Now + ".xlsx");
        }

        private int UserId()
        {
            return Convert.ToInt32(User.Claims.ElementAt(0).Value);
        }
    }
}
