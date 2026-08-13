# 1. Etapa de Execução (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. Etapa de Compilação (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia e restaura apontando para a subpasta
COPY ["Rastreador/Rastreador.csproj", "Rastreador/"]
RUN dotnet restore "Rastreador/Rastreador.csproj"

# Copia todo o código e faz a build
COPY . .
WORKDIR "/src/Rastreador"
RUN dotnet build "Rastreador.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Etapa de Publicação (Publish)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Rastreador.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Imagem Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rastreador.dll"]