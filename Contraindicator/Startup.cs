using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contraindicator.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neo4jClient;
using Swashbuckle.AspNetCore.Swagger;

namespace Contraindicator
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
            //Adding GraphClient
            services.AddSingleton<IGraphClientFactory>(factory =>
                                                    new GraphClientFactory(
                                                        NeoServerConfiguration.GetConfiguration(new Uri(Configuration["Graph:Uri"]), Configuration["Graph:Username"], Configuration["Graph:Password"])
                                                        ));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Adding Configuration
            services.AddSingleton(Configuration);

            //Adding Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Contraindicator API", Version = "v1" });
            });

            //Adding Logging
            services.AddLogging();

            //Adding GraphClientRepository
            services.AddScoped<IGraphClientRepository, GraphClientRepository>();

            //Adding Seed Data
            services.AddTransient<IGraphClientSeedData, GraphClientSeedData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IGraphClientSeedData seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Sample Web API");
                c.RoutePrefix = "api/swagger";

            });

            app.UseHttpsRedirection();
            app.UseMvc();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            seeder.EnsureSeedDataAsync().Wait();
        }
    }
}
