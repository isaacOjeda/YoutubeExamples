using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using Microsoft.Extensions.Configuration;

namespace MyFirstApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyFirstApiDbContext>(options =>
            {
                if (Configuration.GetValue<bool>("UseInMemory"))
                {
                    options.UseInMemoryDatabase("MyFirstApiInMemoryDb");
                }
                else
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                }
            });

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "My First API";
            });

            services.AddCors(options =>
                {
                    options.AddPolicy("MyFirstApiCors", builder => builder
                        .WithOrigins("http://localhost:5002", "https://localhost:5003")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .Build()
                    );
                });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi()
                .UseSwaggerUi3(configure =>
                {
                    configure.Path = "/api";
                });

            app.UseCors("MyFirstApiCors");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
