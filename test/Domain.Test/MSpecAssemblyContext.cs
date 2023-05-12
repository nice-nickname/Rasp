using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Incoding.Data.NHibernate;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;
using Microsoft.Extensions.Configuration;
using NHibernate.Tool.hbm2ddl;

namespace Domain.Test;

public class MSpecAssemblyContext : IAssemblyContext
{
    public void OnAssemblyStart()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .AddEnvironmentVariables()
            .Build();

        var nhConfig = Fluently.Configure()
                               .Database(MsSqlConfiguration.MsSql2008.ConnectionString(config["ConnectionString"]).ShowSql())
                               .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                               .Mappings(configuration => configuration.FluentMappings.AddFromAssembly(typeof(Bootstrap).Assembly));

        PleasureForData.Factory = () => new NhibernateUnitOfWorkFactory(new NhibernateSessionFactory(nhConfig));
    }

    public void OnAssemblyComplete() { }
}
