FROM node:latest AS node_base
RUN echo "NODE Version:" && node --version
RUN echo "NPM Version:" && npm --version

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 5136

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY src/UI/UI.csproj src/UI/
RUN dotnet restore src/UI/UI.csproj
COPY . .
WORKDIR /src/UI
RUN dotnet build UI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish UI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=node_base /app .
RUN npm ci
ENTRYPOINT ["dotnet", "DockerServiceDemo.dll"]