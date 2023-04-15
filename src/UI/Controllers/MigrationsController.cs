﻿using FluentMigrator.Runner;
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

    [Route("up")]
    public IActionResult MigrateUp()
    {
        this._migrator.MigrateUp();
        return Ok("migrated");
    }

    [Route("up/{number:long}")]
    public IActionResult Up(long number)
    {
        this._migrator.MigrateUp(number);
        return Ok($"migrated to {number}");
    }

    [Route("down")]
    public IActionResult MigrateDown()
    {
        this._migrator.MigrateDown(0);
        return Ok("migrated down");
    }

    [Route("down/{number:long}")]
    public IActionResult Down(long number)
    {
        return Ok($"migrated down to {number}");
    }
}
