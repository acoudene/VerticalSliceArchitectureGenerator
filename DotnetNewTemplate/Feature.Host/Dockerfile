#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Feature.Host/Feature.Host.csproj", "Feature.Host/"]
COPY ["Feature.Api/Feature.Api.csproj", "Feature.Api/"]
COPY ["Core.Api/Core.Api.csproj", "Core.Api/"]
COPY ["Core.Data/Core.Data.csproj", "Core.Data/"]
COPY ["Core.Dtos/Core.Dtos.csproj", "Core.Dtos/"]
COPY ["Feature.Data/Feature.Data.csproj", "Feature.Data/"]
COPY ["Feature.Dtos/Feature.Dtos.csproj", "Feature.Dtos/"]
COPY ["Feature.Data.MongoDb/Feature.Data.MongoDb.csproj", "Feature.Data.MongoDb/"]
COPY ["Core.Data.MongoDb/Core.Data.MongoDb.csproj", "Core.Data.MongoDb/"]
RUN dotnet restore "./Feature.Host/./Feature.Host.csproj"
COPY . .
WORKDIR "/src/Feature.Host"
RUN dotnet build "./Feature.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Feature.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feature.Host.dll"]