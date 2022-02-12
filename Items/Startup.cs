using FluentMigrator.Runner;
using Items.Database;
using Items.Database.Repositories;
using Items.Database.Repositories.IRepositories;
using Items.Migrations;
using Items.Security;
using Items.Service;
using Items.Service.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Items
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
            services.AddSingleton<DbContext>();
            services.AddSingleton<MigrationDb>();

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IItemsRepository, ItemsRepository>();
            services.AddScoped<IRegionsRepository, RegionsRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            
            services.AddScoped<IAuthService,AuthService>();
            services.AddScoped<IItemsService,ItemsService>();
            services.AddScoped<IRegionsService,RegionsService>();
            services.AddScoped<IOrdersService,OrdersService>();


            services.AddLogging(c => c.AddFluentMigratorConsole())
                  .AddFluentMigratorCore()
                  .ConfigureRunner(c => c.AddSqlServer2016()
                  .WithGlobalConnectionString(Configuration.GetConnectionString("DapperConnection"))
                  .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()
                  );


            services.AddControllers();
            services.AddSwaggerGen(options=> {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Items API",
                    Description = "API для работы с товарами",
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Need token",
                    Name = "Authorization",
                    Type =  SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

                //Determine base path for the application.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                //Set the comments path for the swagger json and ui.
                var xmlPath = Path.Combine(basePath, "Items.xml");
                options.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("ValidationToken:secret"))),
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetValue<string>("ValidationToken:issuer"),
                        ValidAudience = Configuration.GetValue<string>("ValidationToken:audience")
                    };

                });

          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("swagger/v1/swagger.json","v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
