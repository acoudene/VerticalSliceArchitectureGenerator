# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Feature.WebApp/Feature.WebApp.csproj", "Feature.WebApp/"]
COPY ["Feature.Api.BackendForFrontend/Feature.Api.BackendForFrontend.csproj", "Feature.Api.BackendForFrontend/"]
COPY ["Feature.Proxies/Feature.Proxies.csproj", "Feature.Proxies/"]
COPY ["Core.Proxying/Core.Proxying.csproj", "Core.Proxying/"]
COPY ["Core.Dtos/Core.Dtos.csproj", "Core.Dtos/"]
COPY ["Feature.Dtos/Feature.Dtos.csproj", "Feature.Dtos/"]
COPY ["Feature.ViewObjects/Feature.ViewObjects.csproj", "Feature.ViewObjects/"]
COPY ["Core.ViewObjects/Core.ViewObjects.csproj", "Core.ViewObjects/"]
COPY ["Feature.ServiceDefaults/Feature.ServiceDefaults.csproj", "Feature.ServiceDefaults/"]
COPY ["Feature.ViewObjects.Validators/Feature.ViewObjects.Validators.csproj", "Feature.ViewObjects.Validators/"]
COPY ["Feature.WebApp.Client/Feature.WebApp.Client.csproj", "Feature.WebApp.Client/"]
COPY ["Feature.Localization/Feature.Localization.csproj", "Feature.Localization/"]
COPY ["Feature.RazorComponents/Feature.RazorComponents.csproj", "Feature.RazorComponents/"]
COPY ["Feature.ViewModels/Feature.ViewModels.csproj", "Feature.ViewModels/"]
COPY ["Core.ViewModels/Core.ViewModels.csproj", "Core.ViewModels/"]
RUN dotnet restore "./Feature.WebApp/Feature.WebApp.csproj"
COPY . .
WORKDIR "/src/Feature.WebApp"
RUN dotnet build "./Feature.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Feature.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feature.WebApp.dll"]