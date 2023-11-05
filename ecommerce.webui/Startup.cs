using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.data.Concrete.EfCore;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace shopapp.webui
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddControllers();
            services.AddControllersWithViews();
            services.AddScoped<IProductRepository, EfCoreProductRepository>();
            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddScoped<ISizeTypeRepository, EfCoreSizeTypeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                RequestPath = "/modules"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "popular-products",
                    pattern: "/Product/PopularProducts",
                    defaults: new { controller = "Product", action = "PopularProducts" }
                );
                endpoints.MapControllerRoute(
                    name: "productCreate",
                    pattern: "/admin/productcreate",
                    defaults: new { controller = "Admin", action = "ProductCreate" }
                );

                endpoints.MapControllerRoute(
                   name: "login",
                   pattern: "/account/login",
                   defaults: new { controller = "Account", action = "Login" }
               );
                endpoints.MapControllerRoute(
                    name: "register",
                    pattern: "/account/register",
                    defaults: new { controller = "Account", action = "Register" }
                );
                endpoints.MapControllerRoute(
                    name: "AdminProductList",
                    pattern: "/admin/productlist",
                    defaults: new { controller = "Admin", action = "ProductList" }
                );
                endpoints.MapControllerRoute(
                    name:"CategoryCreate",
                    pattern:"/Admin/CategoryCreate",
                    defaults: new {controller="Admin", action="CategoryCreate"}
                );
                 endpoints.MapControllerRoute(
                    name:"CategoryEdit",
                    pattern:"/Admin/CategoryEdit/{id}",
                    defaults: new {controller="Admin", action="CategoryEdit"}
                );
                  endpoints.MapControllerRoute(
                    name:"CategoryDelete",
                    pattern:"/Admin/CategoryDelete/{id}",
                    defaults: new {controller="Admin", action="CategoryDelete"}
                );
                  endpoints.MapControllerRoute(
                    name: "productEdit",
                    pattern: "/admin/productedit/{id}",
                    defaults: new { controller = "Admin", action = "ProductEdit" }
                );
                endpoints.MapControllerRoute(
                name: "productDelete",
                pattern: "/Admin/ProductDelete/{id}",
                defaults: new { controller = "Admin", action = "ProductDelete" }
               );
                endpoints.MapControllerRoute(
                    name: "productDetail",
                    pattern: "/product/details/{id}",
                    defaults: new { controller = "Product", action = "Details" }
                );

                endpoints.MapControllerRoute(
                    name: "productWithCategory",
                    pattern: "/products/{category?}/{genderId?}",
                    defaults: new { controller = "Product", action = "List" }
                );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                    );

            });

        }
    }
}
