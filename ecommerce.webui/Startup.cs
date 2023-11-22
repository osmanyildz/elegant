using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.data.Concrete.EfCore;
using ecommerce.webui.EmailServices;
using ecommerce.webui.Identity;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;


namespace shopapp.webui
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddControllers();
            services.AddDbContext<ApplicationContext>(options=> options.UseSqlite("Data Source = ecommerceDb"));
            services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders(); //IdentityRole hazır sınıfını kullanan bir User sınıfı ekledik. Hazır User'a firstname ve lastname alanı ekleyebilelim diye. 
            services.AddControllersWithViews();
            services.AddScoped<IProductRepository, EfCoreProductRepository>();
            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddScoped<ISizeTypeRepository, EfCoreSizeTypeRepository>();
            services.AddScoped<ICartRepository, EfCoreCartRepository>();

            services.AddScoped<IEmailSender, SmtpEmailSender>(i=>
            new SmtpEmailSender(
                _configuration["EmailSender:Host"],
                _configuration.GetValue<int>("EmailSender:Port"),
                _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                _configuration["EmailSender:UserName"],
                _configuration["EmailSender:Password"]
                )
            );

            services.Configure<IdentityOptions>(options => {
                // password
                // options.Password.RequireDigit=true; //true ise şifrede sayı olmalı
                // options.Password.RequireLowercase=true; //true ise şifrede küçük harf olmalı
                // options.Password.RequireUppercase=true; //true ise şifrede büyük harf olmalı
                // options.Password.RequiredLength=6; //min 6 karakterlik parola
                // options.Password.RequireNonAlphanumeric=true;
                
                options.User.RequireUniqueEmail=true;
            
                options.SignIn.RequireConfirmedEmail=true;
                options.SignIn.RequireConfirmedPhoneNumber=false;

            });
            services.ConfigureApplicationCookie(options=>{
                options.LoginPath="/Account/Login"; //Eğer session ile cookie birbirini tanımıyorsa uygulamanın kullanıcıyı yönlendireceği path
                options.LogoutPath="/account/logout";
                options.AccessDeniedPath= "/account/accessdenied";
                options.SlidingExpiration=true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder(){
                    HttpOnly=true,
                    Name=".Ecommerce.Security.Cookie"
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
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
                    name: "role-list",
                    pattern: "/admin/RoleList",
                    defaults: new { controller = "Admin", action = "RoleList" }
                );
                endpoints.MapControllerRoute(
                    name: "cart-index",
                    pattern: "/cart/Index",
                    defaults: new { controller = "Cart", action = "Index" }
                );

                endpoints.MapControllerRoute(
                    name: "addToCart",
                    pattern: "/cart/addtoCart",
                    defaults: new { controller = "Cart", action = "AddToCart" }
                );
                endpoints.MapControllerRoute(
                    name: "addOneToCart",
                    pattern: "/cart/AddOne",
                    defaults: new { controller = "Cart", action = "AddOne" }
                );
                  endpoints.MapControllerRoute(
                    name: "removeOneToCart",
                    pattern: "/cart/RemoveOne",
                    defaults: new { controller = "Cart", action = "RemoveOne" }
                );
                  endpoints.MapControllerRoute(
                    name: "payPage",
                    pattern: "/cart/Checkout",
                    defaults: new { controller = "Cart", action = "Checkout" }
                );
                endpoints.MapControllerRoute(
                    name: "cart-delete-item",
                    pattern: "/cart/DeleteCartItem",
                    defaults: new { controller = "Cart", action = "DeleteCartItem" }
                );
                endpoints.MapControllerRoute(
                    name: "adminrolecreate",
                    pattern:"/admin/RoleCreate",
                    defaults: new {controller="Admin",action="RoleCreate"}
                );
                
              
                endpoints.MapControllerRoute(
                    name: "adminuseredit",
                    pattern:"/admin/UserEdit/{id?}",
                    defaults: new {controller="Admin",action="UserEdit"}
                );
                endpoints.MapControllerRoute(
                    name: "adminuserdelete",
                    pattern:"/admin/UserDelete/{id}",
                    defaults: new {controller="Admin",action="UserDelete"}
                );
                
                endpoints.MapControllerRoute(
                    name:"adminroledelete",
                    pattern: "/admin/RoleDelete/{id}",
                    defaults:new {controller="Admin",action="RoleDelete"}
                );
                  endpoints.MapControllerRoute(
                    name: "adminroleedit", 
                    pattern: "/admin/role/{id?}",
                    defaults: new {controller="Admin",action="RoleEdit"}
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
