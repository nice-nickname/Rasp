FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY src/UI/UI.csproj src/UI/
COPY src/Domain/Domain.csproj src/Domain/
COPY src/Resources/Resources.csproj src/Resources/
COPY test/Domain.Test/Domain.Test.csproj test/Domain.Test/
RUN dotnet restore src/UI/UI.csproj
RUN dotnet restore src/Domain/Domain.csproj 
RUN dotnet restore src/Resources/Resources.csproj
RUN dotnet restore test/Domain.Test/Domain.Test.csproj
COPY . .
RUN dotnet build src/UI/UI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish -c release -o /app --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN apt-get update
RUN apt-get update
RUN apt-get -y install nodejs
RUN apt-get -y install npm
RUN npm ci
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "UI.dll"]
