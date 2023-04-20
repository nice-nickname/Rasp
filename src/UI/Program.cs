﻿using FluentMigrator.Runner;
using FluentNHibernate.Cfg.Db;
using FluentValidation;
using FluentValidation.AspNetCore;
using Incoding.Core;
using Incoding.Core.Block.Caching;
using Incoding.Core.Block.Caching.Providers;
using Incoding.Core.Block.IoC;
using Incoding.Core.Block.IoC.Provider;
using Incoding.Core.CQRS;
using Incoding.Core.Data;
using Incoding.Data.EF;
using Incoding.Web;
using Incoding.Web.MvcContrib;
using Microsoft.Extensions.Caching.Memory;
using NUglify.JavaScript;
using Domain.Api;
using Domain.Api.Building;

namespace UI;

using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;

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
               .ConfigureRunner(r => r.AddSqlServer2012()
                                      .WithGlobalConnectionString(builder.Configuration["ConnectionString"])
                                      .ScanIn(typeof(Domain.Bootstrap).Assembly).For.Migrations());

        builder.Services.AddRouting();

        builder.Services
               .AddMvc(o => o.Filters.Add(new IncodingErrorHandlingFilter()))
               .AddFluentValidation(config =>
               {
                   config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                   config.ValidatorFactory = new IncValidatorFactory();
                   AssemblyScanner.FindValidatorsInAssembly(typeof(Domain.Bootstrap).Assembly).ForEach(r => 
                   {    
                        builder.Services.Add(ServiceDescriptor.Transient(r.InterfaceType, r.ValidatorType));
                        builder.Services.Add(ServiceDescriptor.Transient(r.ValidatorType, r.ValidatorType));
                   });
               });

        builder.Services.ConfigureIncodingCoreServices();

        builder.Services.ConfigureIncodingNhDataServices(typeof(IncEntityBase),
                                                         null,
                                                         fluentConfig =>
                                                         {
                                                             var db = MsSqlConfiguration.MsSql2012.ConnectionString(builder.Configuration["ConnectionString"]).ShowSql();
                                                             fluentConfig = fluentConfig.Database(db)
                                                                                        .Mappings(m => m.FluentMappings.AddFromAssembly(typeof(Domain.Bootstrap).Assembly))
                                                                                        .ExposeConfiguration(c => SchemaMetadataUpdater.QuoteTableAndColumns(c, new MsSql2012Dialect()));
                                                             return fluentConfig;
                                                         });

        builder.Services.ConfigureIncodingWebServices();

        builder.Services.AddWebOptimizer(pipeline =>
        {
            // Main libraries
            pipeline.AddJavaScriptBundle("/lib/jq.js",
                                         new CodeSettings { MinifyCode = true },
                                         "node_modules/jquery/dist/jquery.min.js",
                                         "wwwroot/lib/jquery.history.js",
                                         "node_modules/**/dist/jquery.form.min.js",
                                         "node_modules/**/dist/jquery.validate.js",
                                         "node_modules/**/dist/jquery.validate.unobtrusive.min.js",
                                         "node_modules/**/dist/jquery.history.min.js",
                                         "node_modules/underscore/underscore-min.js",
                                         "node_modules/**/dist/handlebars.min.js")
                    .UseContentRoot();

            pipeline.AddJavaScriptBundle("/lib/inc.js",
                                         new CodeSettings { MinifyCode = false },
                                         "/lib/incoding.framework.js");

            // Dev scripts
            pipeline.AddJavaScriptBundle("/lib/script.js", new CodeSettings { MinifyCode = false }, "/js/**/*.js");

            pipeline.AddCssBundle("/css/bootstrap.css", "node_modules/bootstrap/dist/css/bootstrap.min.css")
                    .UseContentRoot();

            pipeline.AddJavaScriptBundle("/lib/bootstrap.js",
                                         new CodeSettings { MinifyCode = true },
                                         "/node_modules/bootstrap/dist/js/bootstrap.bundle.min.js")
                    .UseContentRoot();

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

        app.UseCookiePolicy();

        app.UseWebOptimizer();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(routeBuilder =>
        {
            routeBuilder.MapControllerRoute("incodingCqrsQuery",
                                            "Cqrs/Query/{incType}",
                                            new
                                            {
                                                    controller = "Dispatcher",
                                                    action = "Query"
                                            });
            routeBuilder.MapControllerRoute("incodingCqrsValidate",
                                            "Cqrs/Validate/{incType}",
                                            new
                                            {
                                                    controller = "Dispatcher",
                                                    action = "Validate"
                                            });
            routeBuilder.MapControllerRoute("incodingCqrsCommand",
                                            "Cqrs/Command/{incTypes}",
                                            new
                                            {
                                                    controller = "Dispatcher",
                                                    action = "Push"
                                            });
            routeBuilder.MapControllerRoute("incodingCqrsRender",
                                            "Cqrs/Render/{incType}",
                                            new
                                            {
                                                    controller = "Dispatcher",
                                                    action = "Render"
                                            });
            routeBuilder.MapControllerRoute("incodingCqrsFile",
                                            "Cqrs/File/{incType}",
                                            new
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
        CachingFactory.Instance.Initialize(cache => cache.WithProvider(new NetCachedProvider(() => app.Services.GetRequiredService<IMemoryCache>())));

        return app;
    }

    public static WebApplication ConfigureIncodingServices(this WebApplication app)
    {
        var d = new DefaultDispatcher();

        try
        {
            d.Push(new PrepareFacultyIfNotExistCommand());
            d.Push(new PrepareDisciplineKindsIfNotExistCommand());
            d.Push(new PrepareSubDisciplineKindsIfNotExistCommand());

            // Автоматическое создание данных чтобы было удобно тестировать

            // Кафедры
            //for (int i = 0; i < 10; i++)
            //{
            //    d.Push(new AddOrEditDepartmentCommand
            //    {
            //            Name = $"department-{i}",
            //            Code = $"d-{i}",
            //            FacultyId = d.Query(new GetFacultiesQuery()).First().Id
            //    });
            //}

            // Корпуса
            //for (int i = 0; i < 10; i++)
            //{
            //    d.Push(new AddOrEditBuildingCommand
            //    {
            //            Name = $"building-{i}"
            //    });
            //}
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, e.Message);
        }

        var ajaxDef = JqueryAjaxOptions.Default;
        ajaxDef.Data = new RouteValueDictionary(new Dictionary<string, object>
        {
                { "FacultyId", Selector.Jquery.Name(GlobalSelectors.FacultyId) }
        });
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
                      .ConfigureIncodingServices()
                      .Run();
    }
}
