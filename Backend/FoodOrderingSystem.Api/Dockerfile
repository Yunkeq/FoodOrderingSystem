# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Hardening / container defaults
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true \
    DOTNET_EnableDiagnostics=0 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy shared build files first (layer caching)
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]

# Copy project files (restore layer)
COPY ["FoodOrderingSystem.Api/FoodOrderingSystem.Api.csproj", "FoodOrderingSystem.Api/"]
COPY ["FoodOrderingSystem.Application/FoodOrderingSystem.Application.csproj", "FoodOrderingSystem.Application/"]
COPY ["FoodOrderingSystem.Domain/FoodOrderingSystem.Domain.csproj", "FoodOrderingSystem.Domain/"]
COPY ["FoodOrderingSystem.Infrastructure/FoodOrderingSystem.Infrastructure.csproj", "FoodOrderingSystem.Infrastructure/"]

# Restore ONLY the API project
RUN dotnet restore "FoodOrderingSystem.Api/FoodOrderingSystem.Api.csproj"

# Copy everything else
COPY . .

# Publish (builds as needed; avoids missing bin/ outputs)
RUN dotnet publish "FoodOrderingSystem.Api/FoodOrderingSystem.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-restore

# Final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Run as non-root (aspnet image provides user 'app')
USER app

ENTRYPOINT ["dotnet", "FoodOrderingSystem.Api.dll"]