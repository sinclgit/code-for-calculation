using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Contracts;
using Repositories;
using Services;
using Data;

[assembly: FunctionsStartup(typeof(calculator.Startup))]
namespace calculator
{

    public class Startup : FunctionsStartup
    {
        private static IConfiguration _configuration = null;

        public IConfiguration Configuration { get; }        

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .SetBasePath(context.ApplicationRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{context.EnvironmentName}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }        
        // public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        // {
        //     var context = builder.GetContext().Configuration;

        //     // optional: customize your configuration sources 
        //     // here, we add appsettings.json files 
        //     // Note that these files are not automatically copied on build or publish. 
        //     //builder.ConfigurationBuilder
        //     //    .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
        //     //    .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false);
        // }


        public override void Configure(IFunctionsHostBuilder builder)
        {
            // get the configuration from the builder
            //var configuration = builder.GetContext().Configuration;

        FunctionsHostBuilderContext context = builder.GetContext();

        // builder.configuration
        //     .AddJsonFile(Path.Combine(context.ApplicationRootPath, "local.settings.json"), optional: true, reloadOnChange: false)
        //     .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"local.settings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
        //     .AddEnvironmentVariables();
        }        


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        // {
        //     if (env.IsDevelopment())
        //     {
        //         app.UseDeveloperExceptionPage();
        //     }

        //     app.UseHttpsRedirection();

        //     app.UseRouting();

        //     // Configure Swagger
        //     app.UseSwagger();
        //     app.UseSwaggerUI(setupAction =>
        //     {
        //         setupAction.SwaggerEndpoint("/swagger/CalculatorOpenAPISpecification/swagger.json", "Calculator API");
        //     });

        //     app.UseAuthorization();

        //     app.UseEndpoints(endpoints =>
        //     {
        //         endpoints.MapControllers();
        //     });
        // }
    }
}