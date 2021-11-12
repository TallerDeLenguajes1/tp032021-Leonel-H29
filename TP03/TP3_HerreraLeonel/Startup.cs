using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP3_HerreraLeonel.Entities;
using NLog.Web;
using TP3_HerreraLeonel.Models;

namespace TP3_HerreraLeonel
{
    public class Startup
    {
        //static List<Cadete> listaCadetes = new List<Cadete>();
        static DBTemporal DB = new DBTemporal(NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger());
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            IDataBase DB_ = new DataBase(Configuration.GetConnectionString("Default"), NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger());           
            services.AddSingleton(DB_);
            
            services.AddSession(options =>
               {
                   options.IdleTimeout = TimeSpan.FromSeconds(3600);
                   options.Cookie.HttpOnly = true;
                   options.Cookie.IsEssential = true;
               } 
            ); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
