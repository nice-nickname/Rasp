using FluentMigrator.Runner;
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

namespace UI;

public static class Startup
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("dbconfig.json", false, true);

        builder.Services
               .AddRazorPages()
               .AddRazorRuntimeCompilation();

        builder.Services
               .AddFluentMigratorCore()
               .ConfigureRunner(r =>
                   r.AddSqlServer2012()
                    .WithGlobalConnectionString(builder.Configuration["ConnectionString"])
                    .ScanIn(typeof(Domain.Bootstrap).Assembly).For.Migrations());

        builder.Services.AddRouting();

        builder.Services
               .AddMvc(o => o.Filters.Add(new IncodingErrorHandlingFilter()))
               .AddFluentValidation(config =>
               {
                   config.ValidatorFactory = new IncValidatorFactory();
                   AssemblyScanner.FindValidatorsInAssembly(typeof(Domain.Bootstrap).Assembly);
               });

        builder.Services.ConfigureIncodingCoreServices();

        builder.Services.ConfigureIncodingNhDataServices(typeof(IncEntityBase), null, b =>
        {
            var db = MsSqlConfiguration.MsSql2012.ConnectionString(builder.Configuration["ConnectionString"]).ShowSql();
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

            // Dev scripts
            pipeline.AddJavaScriptBundle("/lib/script.js", new CodeSettings { MinifyCode = false }, "/js/**/*.js");
            
            // Dev styles
            pipeline.AddCssBundle("/css/styles.css", "/css/**/*.css");
        });

        return builder.Build();
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
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

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(routeBuilder =>
        {
            routeBuilder.MapControllerRoute("incodingCqrsQuery", "Cqrs/Query/{incType}", new
            {
                controller = "Dispatcher",
                action = "Query"
            });
            routeBuilder.MapControllerRoute("incodingCqrsValidate", "Cqrs/Validate/{incType}", new
            {
                controller = "Dispatcher",
                action = "Validate"
            });
            routeBuilder.MapControllerRoute("incodingCqrsCommand", "Cqrs/Command/{incTypes}", new
            {
                controller = "Dispatcher",
                action = "Push"
            });
            routeBuilder.MapControllerRoute("incodingCqrsRender", "Cqrs/Render/{incType}", new
            {
                controller = "Dispatcher",
                action = "Render"
            });
            routeBuilder.MapControllerRoute("incodingCqrsFile", "Cqrs/File/{incType}", new
            {
                controller = "Dispatcher",
                action = "QueryToFile"
            });
            routeBuilder.MapDefaultControllerRoute();
        });

        app.Services
           .CreateScope()
           .ServiceProvider
           .GetRequiredService<IMigrationRunner>()
           .MigrateUp();

        IoCFactory.Instance.Initialize(ioc => ioc.WithProvider(new MSDependencyInjectionIoCProvider(app.Services)));
        CachingFactory.Instance.Initialize(cache =>
            cache.WithProvider(new NetCachedProvider(() => app.Services.GetRequiredService<IMemoryCache>())));

        return app;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        WebApplication.CreateBuilder(args)
                      .ConfigureServices()
                      .ConfigureApplication()
                      .Run();
    }
}