using BethanysPieShop.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        
        public IConfiguration ConfigurationRoot { get; set; }

        public Startup(IConfiguration configurationRoot)
        {
            ConfigurationRoot = configurationRoot;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //register services here through Dependecny Injection.

            services.AddDbContext<AppDbContext>(options =>
                                options.UseSqlServer(ConfigurationRoot.GetConnectionString("DefaultConnection")));


            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AppDbContext>(); // to support ASP.NET Identity
            services.AddScoped<IPieRepository, PieRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddControllersWithViews(); // => to support MVC Pattern in our app.

            //Theses services are used to support session and guarantee associate session with incoming request.
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));
            services.AddHttpContextAccessor();
            services.AddSession();
            services.AddRazorPages(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Here we set up request pipeline which is called middlware components(number of componnets chained)
            // Big Tips Here: order of compnents is matter.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // we check if is env is Development or production or stage.
            }

            app.UseHttpsRedirection(); // we need our app runs on HTTPS not http.
            app.UseStaticFiles(); // we need our app to serve files like css and javascript files and images, this middleware search about files which placed in wwwwroot folder.

            app.UseSession(); // for supporting session

            app.UseRouting(); // To support routing and can select rigth controller for incoming request
            app.UseAuthentication(); //To support ASP.NET Identity
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute( // here responsible for map incoming request with an action controller.
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
