#!/bin/bash
set -e

echo "Aguardando PostgreSQL ficar disponível..."
sleep 5

echo "Aplicando migrações do banco de dados..."
dotnet-ef database update --verbose

echo "Iniciando a aplicação..."
exec dotnet sistema_locacao_motos.dll