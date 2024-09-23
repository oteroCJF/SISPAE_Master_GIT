using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Sispae.Interfaces;
using Sispae.Repositories;
using Sispae.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SISPAEV2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddScoped<IRepositorioLogin, RepositorioLogin>();
            services.AddScoped<IRepositorioUsuarios, RepositorioUsuarios>();
            services.AddScoped<IRepositorioAreas, RepositorioAreas>();
            services.AddScoped<IRepositorioInmuebles, RepositorioInmuebles>();
            services.AddScoped<IRepositorioPerfiles, RepositorioPerfiles>();
            services.AddScoped<IRepositorioModulos, RepositorioModulos>();
            services.AddScoped<IRepositorioOperaciones, RepositorioOperaciones>();
            services.AddScoped<IRepositorioOperacionesPerfil, RepositorioOperacionesPerfil>();
            services.AddScoped<IRepositorioProyectos, RepositorioProyectos>();
            services.AddScoped<IRepositorioIntegracion, RepositorioIntegracion>();
            services.AddScoped<IRepositorioUnidadesEjecutoras, RepositorioUnidadesEjecutoras>();
            services.AddScoped<IRepositorioProyectosUnidad, RepositorioProyectosUnidad>();
            services.AddScoped<IRepositorioPartidasPresupuestales, RepositorioPartidasPresupuestales>();
            services.AddScoped<IRepositorioHistoriales, RepositorioHistoriales>();
            services.AddScoped<IRepositorioReportesUEG, RepositorioReportesUEG>();
            services.AddScoped<IRepositorioSeguimiento, RepositorioSeguimiento>();
            services.AddScoped<IRepositorioPrestadorServicios, RepositorioPrestadorServicios>();
            services.AddScoped<IRepositorioRecursos, RepositorioRecursos>();
            services.AddScoped<IRepositorioEntregables, RepositorioEntregables>();
            services.AddScoped<IRepositorioDashboards, RepositorioDashboards>();
            services.AddScoped<IRepositorioAhorros, RepositorioAhorros>();
            services.AddScoped<IRepositorioIntegracion, RepositorioIntegracion>();
            services.AddScoped<IRepositorioCatalogoProyectos, RepositorioCatalogoProyectos>();
            services.AddScoped<IRepositorioRecursosProyecto, RepositorioRecursosProyecto>();
            services.AddScoped<IRepositorioCTTipoProyecto, RepositorioCTTipoProyecto>();



            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
                options.LoginPath = "/login";

            });

            //Services
            services.AddTransient<ModuleService>();
            services.AddTransient<PermisosService>();

            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
