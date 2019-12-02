using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BookShelfApp.DbContextConfig;
using BookShelfApp.Managers;

namespace BookShelfApp
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<DbContext>();
            services.AddTransient<IUserManager, UserManager>();
            string connStr = Configuration.GetConnectionString("BookShelfAppDbContext");
            services.AddDbContext<BookShelfAppDbContext>(opt =>

            opt.UseSqlServer(connStr, builder =>
            {
                builder.UseRowNumberForPaging();
            }));


            // Register the Swagger services
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "BookshelfApp API";
                    document.Info.Description = "A simple ASP.NET Core web API";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Sebig Solutions",
                        Email = string.Empty,
                        Url = "https://twitter.com/DevChooks"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(_defaultCorsPolicyName);
            app.UseCors(builder => builder.AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowAnyOrigin()
                                  .AllowCredentials());


            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // app.UseMiddleware<AuthenticationMiddleware>();

            // app.UseRequestLogMiddleware();

            app.UseMvc();
        }
    }
}
