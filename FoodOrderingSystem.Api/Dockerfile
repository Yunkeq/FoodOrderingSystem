# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution + shared build files first (better layer caching)
COPY ["FoodOrderingSystem.sln", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]

# Copy project files (restore layer)
COPY ["FoodOrderingSystem.Api/FoodOrderingSystem.Api.csproj", "FoodOrderingSystem.Api/"]
COPY ["FoodOrderingSystem.Application/FoodOrderingSystem.Application.csproj", "FoodOrderingSystem.Application/"]
COPY ["FoodOrderingSystem.Domain/FoodOrderingSystem.Domain.csproj", "FoodOrderingSystem.Domain/"]
COPY ["FoodOrderingSystem.Infrastructure/FoodOrderingSystem.Infrastructure.csproj", "FoodOrderingSystem.Infrastructure/"]

RUN dotnet restore "./FoodOrderingSystem.sln"

# Copy everything else and build
COPY . .
RUN dotnet build "./FoodOrderingSystem.Api/FoodOrderingSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FoodOrderingSystem.Api/FoodOrderingSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# If your environment provides APP_UID, you can uncomment the next line.
# USER $APP_UID

ENTRYPOINT ["dotnet", "FoodOrderingSystem.Api.dll"]