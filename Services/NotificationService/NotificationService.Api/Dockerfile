﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NotificationService.Api/NotificationService.Api.csproj NotificationService.Api/
COPY NotificationService.Application/NotificationService.Application.csproj NotificationService.Application/
COPY NotificationService.Infrastructure/NotificationService.Infrastructure.csproj NotificationService.Infrastructure/
COPY NotificationService.Domain/NotificationService.Domain.csproj NotificationService.Domain/

RUN dotnet restore "NotificationService.Api/NotificationService.Api.csproj"

COPY . .

WORKDIR "/src/NotificationService.Api"
RUN dotnet build "NotificationService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Api.dll"]
