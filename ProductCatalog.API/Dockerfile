#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ProductCatalog.API/ProductCatalog.API.csproj", "ProductCatalog.API/"]
COPY ["ProductCatalog.Application/ProductCatalog.Application.csproj", "ProductCatalog.Application/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["ProductCatalog.Domain/ProductCatalog.Domain.csproj", "ProductCatalog.Domain/"]
COPY ["ProductCatalog.Persistence/ProductCatalog.Persistence.csproj", "ProductCatalog.Persistence/"]
RUN dotnet restore "ProductCatalog.API/ProductCatalog.API.csproj"
COPY . .
WORKDIR "/src/ProductCatalog.API"
RUN dotnet build "ProductCatalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductCatalog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalog.API.dll"]