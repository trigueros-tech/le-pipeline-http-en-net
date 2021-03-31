using ExampleApi.Data;
using ExampleApi.Infrastructure.AspNetCore;
using ExampleApi.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ExampleApi
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
            services.AddDbContext<ApplicationContext>(options => { options.UseSqlite(@"Data Source=database.db"); });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ExampleApi", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Crée la base de données
            using (var scope = app.ApplicationServices.CreateScope())
            using (var context = scope.ServiceProvider.GetService<ApplicationContext>())
            {
                context.Database.EnsureCreated();
            }

            
            if (env.IsDevelopment())
            {
                app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExampleApi v1"));
            }

            // si la connexion est en http, redirige vers https
            app.UseHttpsRedirection();
            
            // Créé une transaction par appel http 
            app.UseMiddleware<UnitOfWorkMiddleware>();
            
            app.UseRouting();

            // valide les droits pour toutes les requetes entrantes
            app.UseAuthorization();

            // Dernière étape : la main est aux contrôleurs
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}