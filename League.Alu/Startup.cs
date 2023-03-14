using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.Configuration.Json;
using League.Api;
using Microsoft.AspNetCore.StaticFiles;
using UEditor.Core;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace League.Alu
{
    public class Startup
    {
        public const string CookieScheme = "Alumni_User";
        public const string LoginPath = "/Login/Index";
        public const string AccessDeniedPath = "/Login/Index";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加Cors服务
            services.AddCors();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            //DbContext 上下文
            League_DbContext.Connection_Strings = AppConfigurtaionServices.Configuration.GetConnectionString("League_DbContext");
            League_DbContext.Migrations_Assembly = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;
            services.AddDbContext<League_DbContext>();

            //MVC,启用运行时编译
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            //UEditor
            services.AddUEditorService();
            //解决中文被编码
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            // Sets the default scheme to cookies
            services.AddAuthentication(CookieScheme).AddCookie(CookieScheme, options =>
            {
                options.AccessDeniedPath = LoginPath;
                options.LoginPath = AccessDeniedPath;
            });
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
            // 添加Cors中间件，开放所有来源的跨域请求访问
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
            //强制执行HTTPS
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".woff"] = "application/x-font-woff";
            provider.Mappings[".woff2"] = "application/x-font-woff";

            //路由中间件
            app.UseRouting();

            // Must go before UseMvc
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), "upload")),
                RequestPath = "/upload",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                }
            });
            // 短路中间件，配置Controller路由
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Framework}/{action=Index}/{id?}");
            });

            //Must be after UseMVC;
            app.UseCookiePolicy();
        }

        public class AppConfigurtaionServices
        {
            public static IConfiguration Configuration { get; set; }
            static AppConfigurtaionServices()
            {
                //ReloadOnChange = true 当appsettings.json被修改时重新加载           
                Configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();
            }
        }
    }
}
