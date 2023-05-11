﻿using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Incoding.Data.NHibernate;
using Incoding.UnitTests.MSpec;
using Machine.Specifications;
using NHibernate.Tool.hbm2ddl;

namespace Domain.Test;

public class MSpecAssemblyContext : IAssemblyContext
{
    public void OnAssemblyStart()
    {
        var nhConfig = Fluently.Configure()
                               .Database(MsSqlConfiguration.MsSql2008.ConnectionString("Server=192.168.0.3,1433;Database=Rasp_Test;User=ahaha;Password=123").ShowSql())
                               .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                               .Mappings(configuration => configuration.FluentMappings.AddFromAssembly(typeof(Bootstrap).Assembly));

        PleasureForData.Factory = () => new NhibernateUnitOfWorkFactory(new NhibernateSessionFactory(nhConfig));
    }

    public void OnAssemblyComplete() { }
}
