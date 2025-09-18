#!/bin/bash

# Quick Start Script for ProductApi
# This script helps you get started with the ProductApi quickly

echo "🚀 ProductApi Quick Start"
echo "========================="
echo

# Check if .NET 8 is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK no está instalado. Por favor instala .NET 8 SDK:"
    echo "   https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

# Check .NET version
DOTNET_VERSION=$(dotnet --version)
echo "✅ .NET SDK detectado: $DOTNET_VERSION"

# Navigate to project directory
cd ProductChallenge

echo "📦 Restaurando paquetes..."
dotnet restore

echo "🔨 Compilando proyecto..."
dotnet build

if [ $? -eq 0 ]; then
    echo "✅ Compilación exitosa"
    echo
    echo "🎯 Próximos pasos:"
    echo "1. Configura la base de datos:"
    echo "   - Ejecuta el script: ProductApi.Infrastructure/Data/ProductDb.sql"
    echo "   - Modifica la cadena de conexión en: ProductApi.API/appsettings.json"
    echo
    echo "2. Ejecuta la API:"
    echo "   dotnet run --project ProductApi.API"
    echo
    echo "3. Prueba los ejemplos:"
    echo "   - Abre: https://localhost:7042/swagger"
    echo "   - Importa: ProductApi-Postman-Collection.json en Postman"
    echo "   - Ejecuta: cd ../ProductApiClient/ProductApiExample && dotnet run"
    echo
    echo "📖 Consulta USAGE_EXAMPLES.md para ejemplos detallados"
else
    echo "❌ Error en la compilación"
    exit 1
fi