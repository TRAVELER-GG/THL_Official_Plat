using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using League.Api;
using System.IO;
using Microsoft.Extensions.FileProviders;
using UEditor.Core;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace League.Plat
{
    public class Startup
    {
        public const string CookieScheme = "Plat_User";
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });


            //DbContext ������
            League_DbContext.Connection_Strings = AppConfigurtaionServices.Configuration.GetConnectionString("League_DbContext");
            League_DbContext.Migrations_Assembly = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;
            services.AddDbContext<League_DbContext>();

            services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;

                o.JsonSerializerOptions.DictionaryKeyPolicy = null;


                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            //MVC,��������ʱ����
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();


            //������ı�����
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Sets the default scheme to cookies
            services.AddAuthentication(CookieScheme).AddCookie(CookieScheme, options =>
            {
                options.AccessDeniedPath = LoginPath;
                options.LoginPath = AccessDeniedPath;
            });
            services.AddUEditorService();
            //ע��Swagger������
            services.AddSwaggerGen(c =>
            {
                //���Swagger�ĵ�
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "League.web.api", Version = "v1" });

                //�ӿ����ע��
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "League_Api.xml");
                c.IncludeXmlComments(filePath);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
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

            //ǿ��ִ��HTTPS
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".woff"] = "application/x-font-woff";
            provider.Mappings[".woff2"] = "application/x-font-woff";

            //·���м��
            app.UseRouting();

            // Must go before UseMvc
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAllOrigins");

            //���Swagger���м��
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //Swagger�ĵ�·��
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "League.web.api");
            });


            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Upload")),
                RequestPath = "/Upload",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                }
            });


            // ��·�м��������Controller·��
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
                //ReloadOnChange = true ��appsettings.json���޸�ʱ���¼���           
                Configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();
            }
        }
    }
}
