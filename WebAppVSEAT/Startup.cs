using BLL;
using DTO;
using DAL;
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

namespace WebAppVSEAT
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
            services.AddScoped<IRestaurantManager, RestaurantManager>();
            services.AddScoped<IRestaurantDB,RestaurantDB>();

            services.AddScoped<IDishManager, DishManager>();
            services.AddScoped<IDishDB, DishDB>();

            services.AddScoped<ICityManager, CityManager>();
            services.AddScoped<ICityDB, CityDB>();

            services.AddScoped<IDishesRestaurantManager, DishesRestaurantManager>();
            services.AddScoped<IDishesRestaurantDB, DishesRestaurantDB>();

            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddScoped<ICustomerDB, CustomerDB>();

            services.AddScoped<IStaffManager, StaffManager>();
            services.AddScoped<IStaffDB, StaffDB>();

            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<IOrderDB, OrderDB>();


            services.AddSession();
            services.AddControllersWithViews();

            
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
            app.UseSession();

            app.UseRouting();

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
