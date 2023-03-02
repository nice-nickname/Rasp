using FluentMigrator.Runner;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[Route("migrator")]
public class MigrationsController : Controller
{
    private readonly IMigrationRunner _migrator;

    public MigrationsController(IMigrationRunner migrator)
    {
        _migrator = migrator;
    }

    [Route("migrate")]
    public void Migrate()
    {
        this._migrator.MigrateUp();
    }

    [Route("up/{number:long}")]
    public void Up(long number)
    {
        this._migrator.MigrateUp(number);
    }

    [Route("down/{number:long}")]
    public void Down(long number)
    {
        this._migrator.MigrateDown(number);
    }
}