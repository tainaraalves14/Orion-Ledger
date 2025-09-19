# Imagem base para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Imagem para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia apenas o csproj primeiro para otimizar o cache do Docker
COPY ["OrionLedger.API/OrionLedger.API.csproj", "OrionLedger.API/"]

# Restaura dependências
RUN dotnet restore "OrionLedger.API/OrionLedger.API.csproj"

# Copia o restante do código
COPY . .

WORKDIR "/src/OrionLedger.API"
RUN dotnet build "OrionLedger.API.csproj" -c Release -o /app/build

# Publicação
FROM build AS publish
RUN dotnet publish "OrionLedger.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagem final (leve)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrionLedger.API.dll"]
