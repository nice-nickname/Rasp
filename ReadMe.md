# Rasp
> Application to create, improve and maintain class schedule 

<p align=center>
    <img src="https://media.discordapp.net/attachments/490986273968881680/1086026627848339456/image.png" alText="Довольные_пользователи" width="500px" style="margin-top: 10px;">    
</p>
<h2 align=center>
Grateful users of our application
</h2>

-----

## Configuration
Rasp uses MS SQL SERVER as primary database. Application requres to have a Database connection, specified in **dbconfig.json**
```json
{
    "ConnectionString": "Your=DB;Connection=String"
}
```
Example for *morons*
```json
{
    "ConnectionString": "Data Source=localhost;Database=Rasp;Integrated Security=true;"
}
```

## Install dependecies
To install all packages type in console
```console
> dotnet restore # Run this command for all Projects in solution

> cd src/UI
src/UI> npm install # This will install all js and css libraries 
```

## Build and run
Standart ASP.NET CORE commands are used to maintain project running

Run application

```console
src/UI>dotnet run
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
