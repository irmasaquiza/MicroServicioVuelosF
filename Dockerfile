FROM mcr.microsoft.com/dotnet/nightly/sdk:10.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore Microservicio.Vuelos.Api/Microservicio.Vuelos.Api.csproj
RUN dotnet publish Microservicio.Vuelos.Api/Microservicio.Vuelos.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/nightly/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Microservicio.Vuelos.Api.dll"]