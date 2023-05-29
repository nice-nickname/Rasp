# Rasp
> Application to create, improve and maintain class schedule 

## Configuration
Rasp uses MS SQL SERVER as primary database. Application requres to have a Database connection, specified in **dbconfig.json**

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
src/UI> npm ci # This will install all js and css libraries 
```

## Build and run
Standart ASP.NET CORE commands are used to maintain project running

Run application

```console
src/UI>dotnet run
```

## Docker configuration
You can use docker compose to create a Docker container with sql server installed and ready-to-work with Rasp

Pull, create, run and attach docker
```console
docker compose up
```
