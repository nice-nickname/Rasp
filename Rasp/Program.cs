using FluentNHibernate.Cfg.Db;
using FluentValidation;
using FluentValidation.AspNetCore;
using Incoding.Core;
using Incoding.Core.Block.Caching;
using Incoding.Core.Block.Caching.Providers;
using Incoding.Core.Block.IoC;
using Incoding.Core.Block.IoC.Provider;
using Incoding.Core.Data;
using Incoding.Data.EF;
using Incoding.Web;
using Incoding.Web.MvcContrib;
using Microsoft.Extensions.Caching.Memory;
using NUglify.JavaScript;

namespace Rasp
{
    public class Program
    {
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services
                   .AddControllersWithViews(o =>
                   {
                       o.Filters.Add(new IncodingErrorHandlingFilter());
                       o.EnableEndpointRouting = false;
                   })
                   .AddFluentValidation(c =>
                   {
                       c.ValidatorFactory = new IncValidatorFactory();
                       AssemblyScanner.FindValidatorsInAssembly(typeof(Domain.Bootstrap).Assembly);
                   });

            builder.Services.ConfigureIncodingCoreServices();

            builder.Services.ConfigureIncodingNhDataServices(typeof(IncEntityBase), null, b =>
            {
                var connectionString = @"Server=localhost,1433;Database=News;User Id=ahaha;Password=123;";
				
				var db = MsSqlConfiguration.MsSql2012.ConnectionString(connectionString).ShowSql();
                b = b.Database(db).Mappings(m => m.FluentMappings.AddFromAssembly(typeof(Domain.Bootstrap).Assembly));
                return b;
            });

            builder.Services.ConfigureIncodingWebServices();

            builder.Services.AddWebOptimizer(pipeline =>
            {
                // Main libraries
                pipeline.AddJavaScriptBundle("/lib/jq.js", new CodeSettings { MinifyCode = true },
                            "node_modules/jquery/dist/jquery.min.js",
                            "wwwroot/lib/jquery.history.js",
                            "node_modules/**/dist/jquery.form.min.js",
                            "node_modules/**/dist/jquery.validate.js",
                            "node_modules/**/dist/jquery.validate.unobtrusive.min.js",
                            "node_modules/**/dist/jquery.history.min.js",
                            "node_modules/underscore/underscore-min.js",
                            "wwwroot/lib/incoding.framework.js",
                            "node_modules/**/dist/handlebars.min.js")
                        .UseContentRoot();
            });
        }

        private static void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseWebOptimizer();
            app.UseStaticFiles();

            app.UseAuthorization();
            app.UseMvc(routes =>
            {
                routes.ConfigureCQRS();
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            IoCFactory.Instance.Initialize(i => i.WithProvider(new MSDependencyInjectionIoCProvider(app.Services)));
            CachingFactory.Instance.Initialize(init =>
                init.WithProvider(new NetCachedProvider(() => app.Services.GetRequiredService<IMemoryCache>())));
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);

            var app = builder.Build();
            Configure(app);

            IoCFactory.Instance.Initialize(init => init.WithProvider(new MSDependencyInjectionIoCProvider(app.Services)));
            CachingFactory.Instance.Initialize(init => init.WithProvider(new NetCachedProvider(() => app.Services.GetRequiredService<IMemoryCache>())));

            app.Run();
        }
    }
}