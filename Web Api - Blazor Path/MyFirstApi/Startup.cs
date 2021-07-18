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
using MyFirstApi.Models;
using Microsoft.AspNetCore.Identity;
using MyFirstApi.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            services.AddIdentityCore<User>(options =>
            {
                // Identity configuration
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<MyFirstApiDbContext>()
            .AddDefaultTokenProviders();

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

            services.AddHttpContextAccessor();
            services.AddAuthentication(AppConstants.JwtScheme)
                .AddJwtBearer(AppConstants.JwtScheme, options =>
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:SecretKey"]));

                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = "myApi/issuer",
                        ValidAudience = "myApi/audience",
                        IssuerSigningKey = securityKey,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddControllers();

            // My Services
            services.AddTransient<MyFirstApiDbContextSeed>();
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
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
