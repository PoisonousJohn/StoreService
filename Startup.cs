using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoreService.Models;
using StoreService.Utils;
using Swashbuckle.AspNetCore.Swagger;

namespace StoreService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(
                typeof(DocumentClient),
                _ => {
                    var dbConf = Configuration.GetSection("DB");
                    var endpoint = dbConf.GetValue<string>("Endpoint");
                    var key = dbConf.GetValue<string>("Key");
                    return new DocumentClient(new Uri(endpoint), key);
                },
                ServiceLifetime.Singleton
            ));

            services.Add(new ServiceDescriptor(
                typeof(DocumentDBRepo<StoreCatalogEntry>),
                s => new DocumentDBRepo<StoreCatalogEntry>(
                    s.GetService<DocumentClient>(),
                    Configuration.GetSection("DB").GetValue<string>("Name"),
                    "Catalog"),
                ServiceLifetime.Singleton
            ));

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Store API", Version = "v1" });
            });
        }

        public static IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}
