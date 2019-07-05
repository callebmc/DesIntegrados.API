using DesIntegrados.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using DesIntegrados.API.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace DesIntegrados.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(option =>
            {
                var defaultPolicy = new CorsPolicyBuilder()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .Build();

                option.AddDefaultPolicy(defaultPolicy);
            });

            services.AddDbContext<DesIntegradosContext>(opt => opt.UseInMemoryDatabase("DesIntegradosDB"));

            services
                .AddMvc()
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
                    opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                })
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                // TODO: Variar esta versão conforme versão do assembly da API.
                c.SwaggerDoc("v1", new Info { Title = "DesIntegrados API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var appContext = serviceScope.ServiceProvider.GetService<DesIntegradosContext>();
                    await appContext.SeedData();
                }
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "ERP NCoreApi v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
