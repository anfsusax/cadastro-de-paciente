# Docker Setup - Be3 Project

Este projeto está configurado para rodar completamente no Docker.

## Pré-requisitos

- Docker Desktop instalado e rodando
- Docker Compose (geralmente incluído no Docker Desktop)

## Estrutura Docker

O projeto consiste em 4 serviços:

1. **sqlserver**: Banco de dados SQL Server 2022
2. **db-init**: Serviço que inicializa o banco de dados com scripts SQL
3. **backend**: API .NET 8.0
4. **frontend**: Aplicação Angular servida via nginx

## Como executar

### 1. Iniciar todos os serviços

```bash
docker-compose up -d
```

Este comando irá:
- Baixar as imagens necessárias (se não estiverem em cache)
- Construir as imagens do backend e frontend
- Iniciar o SQL Server
- Executar os scripts de inicialização do banco
- Iniciar o backend e frontend

### 2. Verificar os logs

```bash
# Ver logs de todos os serviços
docker-compose logs -f

# Ver logs de um serviço específico
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f sqlserver
```

### 3. Acessar a aplicação

- **Frontend**: http://localhost:4200
- **Backend API**: http://localhost:5123
- **Swagger**: http://localhost:5123/swagger
- **SQL Server**: localhost:1433

### 4. Parar os serviços

```bash
docker-compose down
```

Para remover também os volumes (dados do banco):

```bash
docker-compose down -v
```

## Credenciais do Banco de Dados

- **Servidor**: localhost,1433 (ou sqlserver dentro da rede Docker)
- **Usuário**: sa
- **Senha**: Be3@Password123!
- **Database**: Be3DB

## Estrutura de Arquivos Docker

```
.
├── docker-compose.yml          # Orquestração dos serviços
├── backend/
│   ├── Dockerfile              # Build da API .NET
│   └── .dockerignore
├── frontend/be3-frontend/
│   ├── Dockerfile              # Build do Angular + nginx
│   ├── nginx.conf              # Configuração do nginx
│   └── .dockerignore
└── database/scripts/            # Scripts SQL de inicialização
    ├── 01_create_database.sql
    ├── 02_create_tables.sql
    ├── 03_seed_convenios.sql
    └── 04_seed_pacientes.sql
```

## Comandos Úteis

### Reconstruir as imagens

```bash
docker-compose build --no-cache
docker-compose up -d
```

### Executar comandos dentro de um container

```bash
# Backend
docker-compose exec backend bash

# Frontend
docker-compose exec frontend sh

# SQL Server
docker-compose exec sqlserver bash
```

### Verificar status dos serviços

```bash
docker-compose ps
```

### Limpar tudo e começar do zero

```bash
docker-compose down -v
docker-compose build --no-cache
docker-compose up -d
```

## Troubleshooting

### Backend não consegue conectar ao banco

1. Verifique se o SQL Server está rodando: `docker-compose ps`
2. Verifique os logs: `docker-compose logs sqlserver`
3. Aguarde alguns segundos após iniciar - o SQL Server pode levar tempo para ficar pronto

### Frontend não carrega

1. Verifique se o backend está rodando: `docker-compose ps`
2. Verifique os logs: `docker-compose logs frontend`
3. Tente acessar diretamente o backend: http://localhost:5123/swagger

### Erro ao inicializar o banco

1. Verifique os logs do db-init: `docker-compose logs db-init`
2. Se necessário, remova o volume e reinicie: `docker-compose down -v && docker-compose up -d`

## Variáveis de Ambiente

As principais variáveis podem ser ajustadas no `docker-compose.yml`:

- `SA_PASSWORD`: Senha do SQL Server
- `ASPNETCORE_ENVIRONMENT`: Ambiente do .NET (Development/Production)
- Portas dos serviços podem ser alteradas nas seções `ports`

## Notas

- O banco de dados persiste em um volume Docker chamado `sqlserver_data`
- O frontend usa nginx com proxy reverso para o backend
- O CORS está configurado para permitir requisições do frontend
- Os scripts SQL são executados automaticamente na primeira inicialização

