using DIO_ApiCatalogoDeJogos_Mongo.Middlewares;
using DIO_ApiCatalogoDeJogos_Mongo.Repositories;
using DIO_ApiCatalogoDeJogos_Mongo.Repositories.Implementations;
using DIO_ApiCatalogoDeJogos_Mongo.Services;
using DIO_ApiCatalogoDeJogos_Mongo.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace DIO_ApiCatalogoDeJogos_Mongo
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DIO_ApiCatalogoDeJogos_Mongo", Version = "v1" });
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var fileName = $"{typeof(Startup).GetTypeInfo().Assembly.GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(basePath, fileName));

            });

            services.AddScoped<IGameRepository, GameMongoDBRepository>();
            services.AddScoped<IGameService, GameService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DIO_ApiCatalogoDeJogos_Mongo v1"));
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
