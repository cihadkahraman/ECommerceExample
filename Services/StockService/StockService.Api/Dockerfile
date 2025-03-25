FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY StockService.Api/StockService.Api.csproj StockService.Api/
COPY StockService.Application/StockService.Application.csproj StockService.Application/
COPY StockService.Domain/StockService.Domain.csproj StockService.Domain/
COPY StockService.Infrastructure/StockService.Infrastructure.csproj StockService.Infrastructure/
COPY StockService.Api/appsettings*.json ./StockService.Api/

RUN dotnet restore "StockService.Api/StockService.Api.csproj"
COPY . .
WORKDIR "/src/StockService.Api"
RUN dotnet build "StockService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockService.Api.dll"]
