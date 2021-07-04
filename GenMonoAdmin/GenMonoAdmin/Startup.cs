using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DatabaseHelper.Controls;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication;
using GenMonoAdmin.Helpers;

namespace GenMonoAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
                options.HttpsPort = 5002;
            });

            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "GenMonoAdminHTTPS", builder =>
                {
                    builder.WithOrigins("*", "*");
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                    builder.SetIsOriginAllowed(origin => true);
                });
            });

            services.AddTransient<IAccountDbHelper, AccountDbHelper>(ad => {
                string ConnectionString = ad.GetService<IConfiguration>()["ConnectionString"];
                return new AccountDbHelper(ConnectionString);
            });

            services.AddTransient<IUserDbHelper, UserDbHelper>(ad => {
                string ConnectionString = ad.GetService<IConfiguration>()["ConnectionString"];
                return new UserDbHelper(ConnectionString);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseCors("GenMonoAdminHTTPS");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
