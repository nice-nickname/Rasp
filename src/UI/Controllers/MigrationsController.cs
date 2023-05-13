using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[Route("migrator")]
[Authorize(Roles = "Rasp.Admin")]
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
        this._migrator.MigrateDown(number);
        return Ok($"migrated down to {number}");
    }

    [Route("refresh/{number:long}")]
    public IActionResult Refresh(long number)
    {
        this._migrator.MigrateDown(number - 1);
        this._migrator.MigrateUp();
        return Ok($"migration {number} refreshed");
    }

    [Route("erase")]
    public IActionResult Erase()
    {
        var sql = @"
DECLARE @Sql NVARCHAR(500) DECLARE @Cursor CURSOR

SET @Cursor = CURSOR FAST_FORWARD FOR
SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_SCHEMA + '].[' +  tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + '];'
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1
LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME =rc1.CONSTRAINT_NAME

OPEN @Cursor FETCH NEXT FROM @Cursor INTO @Sql

WHILE (@@FETCH_STATUS = 0)
BEGIN
Exec sp_executesql @Sql
FETCH NEXT FROM @Cursor INTO @Sql
END

CLOSE @Cursor DEALLOCATE @Cursor
GO

EXEC sp_MSforeachtable 'DROP TABLE ?'
GO";

        this._migrator.Processor.Execute(sql);
        return Ok("all data has been erased");
    }
}
