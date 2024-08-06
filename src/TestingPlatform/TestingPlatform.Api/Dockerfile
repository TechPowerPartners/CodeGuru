#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TestingPlatform/TestingPlatform.Api/TestingPlatform.Api.csproj", "src/TestingPlatform/TestingPlatform.Api/"]
COPY ["src/TestingPlatform/TestingPlatform.Api.Contracts/TestingPlatform.Api.Contracts.csproj", "src/TestingPlatform/TestingPlatform.Api.Contracts/"]
COPY ["src/TestingPlatform/TestingPlatform.Domain/TestingPlatform.Domain.csproj", "src/TestingPlatform/TestingPlatform.Domain/"]
RUN dotnet restore "./src/TestingPlatform/TestingPlatform.Api/TestingPlatform.Api.csproj"
COPY . .
WORKDIR "/src/src/TestingPlatform/TestingPlatform.Api"
RUN dotnet build "./TestingPlatform.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TestingPlatform.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestingPlatform.Api.dll"]