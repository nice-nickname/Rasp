# Rasp
> Application to create, improve and maintain class schedule 
## Configuration
Rasp uses MS SQL SERVER as primary database. Application requres to have a Database connection, specified in **dbconfig.json**
```json
{
    "ConnectionString": "Your=DB;Connection=String"
}
```
## Build and run
Standart ASP.NET CORE commands are used to maintain project building

Run application

```console
dotnet run app.exe
```

Build application

```console
dotnet build app.exe
```