# Rasp
> Application to create, improve and maintain class schedule 

<p align=center>
    <img src="https://media.discordapp.net/attachments/490986273968881680/1086026627848339456/image.png" alText="Довольные_пользователи" width="500px" style="margin-top: 10px;">    
</p>
<h2 align=center>
Grateful users of out application
</h2>

-----

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
dotnet run
```

Build application

```console
dotnet build
```

## Docker configuration
You can use docker compose to create a Docker container with sql server installed and ready-to-work with Rasp

Pull, create, run and attach docker
```console
docker compose up
```
![Чурки](http://ii.yakuji.moe/b/src/1592057938362.png)
