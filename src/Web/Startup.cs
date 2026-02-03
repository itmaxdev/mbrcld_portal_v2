using AspNetCoreRateLimit;
using ChatData.Context;
using Mbrcld.Application;
using Mbrcld.Application.Extensions;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Web.Extensions;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Linq;

namespace Mbrcld
{
    public class Startup
    {
        private const string DEFAULT_CON_STR = "Default";

        private readonly string connectionString;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration.GetConnectionString(DEFAULT_CON_STR);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddOData();
            services.AddSignalR().AddMessagePackProtocol();
            services.AddControllers();
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
                                     .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddDbContext<ChatContext>(options => options.UseSqlServer(connectionString));
            services.AddCors((options) =>
            {
                options.AddDefaultPolicy(
                    (builder) =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod();
                    });
            });

            services.ConfigureApiVersioning();
            services.ConfigureAspNetCoreIdentity();
            services.ConfigureIdentityServer();
            services.ConfigureJwtBearerAuthentication(Configuration);
            services.ConfigureAuthorization();
            services.ConfigureRateLimiting(Configuration);
            services.ConfigureControllersWithViews();
            services.ConfigureSwagger(Configuration);
            services.ConfigureUrlHelper();
            services.AddConfigOptions(Configuration);

            services.AddApplication();
            services.AddInfrastructure(this.Configuration);
            services.ConfigureServices();


            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseProblemDetails();
            app.UseIpRateLimiting();

            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseIdentityServer();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI((options) =>
                {
                    options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "MBRCLD API v1.0");
                    options.OAuthAppName("MBRCLD API - Swagger");
                    options.OAuthClientId("dev");
                });
            }

            app.UseCors();

            app.UseResponseCaching();

            app.UseApiVersioning();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            // Todo uncomment before deployment
            using (var context = scope.ServiceProvider.GetService<ChatContext>())
               if(context.Database.CanConnect()) 
                    context.Database.Migrate();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.EnableDependencyInjection();
                endpoints
                    .Select()
                    .Filter()
                    .OrderBy()
                    .Count()
                    .Expand()
                    .MaxTop(1000);
                endpoints.MapHub<ChatHub>("/chatHub");
            });

            app.UseSpaMultiLanguage();

            app.Map("/ar", (ar) =>
            {
                ar.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.Options.DefaultPage = "/ar/index.html";

                    if (env.IsDevelopment())
                    {
                        spa.UseProxyToSpaDevelopmentServer("http://localhost:4201");
                    }
                });
            });

            app.Map("/en", (ar) =>
            {
                ar.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.Options.DefaultPage = "/en/index.html";

                    if (env.IsDevelopment())
                    {
                        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    }
                });
            });
        }
    }
}
