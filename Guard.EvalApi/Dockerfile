FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
#EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Guard.EvalApi/Guard.EvalApi.csproj", "Guard.EvalApi/"]
RUN dotnet restore "Guard.EvalApi/Guard.EvalApi.csproj"
COPY . .
WORKDIR "/src/Guard.EvalApi"
RUN dotnet build "Guard.EvalApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Guard.EvalApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Guard.EvalApi.dll"]
