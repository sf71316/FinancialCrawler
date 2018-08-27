using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Financial.Data.interfaces;
using Financial.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Unity;
using Unity.Injection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Financial.API
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 ValidIssuer = Configuration["Tokens:ValidIssuer"],
                 ValidAudience = Configuration["Tokens:ValidAudience"],
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:IssuerSigningKey"])),
                 RequireExpirationTime = true,
             };
             options.Events = new JwtBearerEvents()
             {
                 OnAuthenticationFailed = context =>
                 {
                     context.NoResult();

                     context.Response.StatusCode = 401;
                     context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = context.Exception.Message;
                     Debug.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                     return Task.CompletedTask;
                 },
                 OnTokenValidated = context =>
                 {
                     Console.WriteLine("OnTokenValidated: " +
                         context.SecurityToken);
                     return Task.CompletedTask;
                 }

             };
         });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        public void ConfigureContainer(IUnityContainer container)
        {
            //var builder = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json");
            //IConfigurationRoot configuration = builder.Build();
            //// Could be used to register more types
            container.RegisterType<ICurrencyData, CurrencyData>(
                new InjectionConstructor(Configuration.GetConnectionString("DefaultConnection")));
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
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
