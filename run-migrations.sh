#!/bin/bash
export PATH=/root/.dotnet/tools:$PATH

# Instala dotnet-ef se ainda não estiver instalado
if ! command -v dotnet-ef &> /dev/null; then
    dotnet tool install --global dotnet-ef --version 9.0.9
fi

# Cria a migration inicial (se necessário)
dotnet ef migrations add InitialCreate \
    --project OrionLedger.Infrastructure/OrionLedger.Infrastructure.csproj \
    --startup-project OrionLedger.API/OrionLedger.API.csproj || true

# Aplica todas as migrations usando a connection string correta
dotnet ef database update \
    --project OrionLedger.Infrastructure/OrionLedger.Infrastructure.csproj \
    --startup-project OrionLedger.API/OrionLedger.API.csproj \
    --connection "Host=db;Database=orionledger;Username=admin;Password=admin123"
