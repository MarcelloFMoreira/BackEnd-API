FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["sistema_locacao_motos/sistema_locacao_motos.csproj", "sistema_locacao_motos/"]
COPY ["slm.infraestrutura/slm.infraestrutura.csproj", "slm.infraestrutura/"]
COPY ["Slm.Domain/Slm.Domain.csproj", "Slm.Domain/"]
RUN dotnet restore "./sistema_locacao_motos/sistema_locacao_motos.csproj"
COPY . .
WORKDIR "/src/sistema_locacao_motos"
RUN dotnet build "./sistema_locacao_motos.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./sistema_locacao_motos.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sistema_locacao_motos.dll"]