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

## Docker configuration
You can use docker compose to create a Docker container with sql server installed and ready-to-work with Rasp

Pull, create, run and attach docker
```console
docker compose up
```
![Чурки](http://ii.yakuji.moe/b/src/1592057938362.png)
