# 🚀 Configuração para Ambientes Local e Docker

Este projeto está configurado para funcionar tanto no ambiente **local** (desenvolvimento) quanto no **Docker**.

## 📋 Ambientes Disponíveis

### 1. Ambiente Local (Desenvolvimento)

Para rodar localmente, você precisa ter:
- .NET 8.0 SDK instalado
- SQL Server ou SQL Server Express instalado localmente
- Node.js e npm instalados
- Angular CLI instalado globalmente (`npm install -g @angular/cli`)

#### Backend Local

1. Configure a connection string no `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=Be3DB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;"
     }
   }
   ```
   > **Nota:** Ajuste o servidor conforme sua instalação (pode ser `localhost` ou `localhost\\SQLEXPRESS`)

2. Execute os scripts SQL na ordem:
   - `database/scripts/01_create_database.sql`
   - `database/scripts/02_create_tables.sql`
   - `database/scripts/03_seed_convenios.sql`
   - `database/scripts/04_seed_pacientes.sql`

3. Inicie o backend:
   ```bash
   cd backend/Be3.Api
   dotnet run
   ```
   O backend estará disponível em: http://localhost:5123

#### Frontend Local

1. Instale as dependências:
   ```bash
   cd frontend/be3-frontend
   npm install
   ```

2. Inicie o servidor de desenvolvimento:
   ```bash
   npm start
   # ou
   ng serve
   ```
   O frontend estará disponível em: http://localhost:4200

#### Configuração Local

- **Backend API:** http://localhost:5123
- **Frontend:** http://localhost:4200
- **Swagger:** http://localhost:5123/swagger
- O frontend usa `environment.ts` que aponta para `http://localhost:5123/api`

### 2. Ambiente Docker

Para rodar no Docker, você só precisa ter o Docker Desktop instalado e rodando.

#### Iniciar no Docker

```bash
# Na raiz do projeto
docker-compose up -d
```

Isso irá:
- Criar e iniciar o SQL Server em container
- Inicializar o banco de dados automaticamente
- Iniciar o backend (sobrescreve a connection string via variável de ambiente)
- Iniciar o frontend (build de produção com nginx)

#### Configuração Docker

- **Frontend:** http://localhost:4200 (nginx com proxy para backend)
- **Backend API:** http://localhost:5123 (acesso direto)
- **Swagger:** http://localhost:5123/swagger
- **SQL Server:** localhost:1433
- O frontend usa `environment.prod.ts` que aponta para `/api` (proxy nginx)

#### Parar o Docker

```bash
docker-compose down
```

Para remover também os volumes (dados do banco):
```bash
docker-compose down -v
```

## 🔄 Como Funciona a Configuração Dual

### Backend

1. **Arquivo base:** `appsettings.json`
   - Connection string padrão: `Server=localhost;Database=Be3DB;Integrated Security=True;...`
   - Usado quando não há variáveis de ambiente

2. **Desenvolvimento local:** `appsettings.Development.json`
   - Sobrescreve com: `Server=localhost\\SQLEXPRESS;...`
   - Carregado automaticamente quando `ASPNETCORE_ENVIRONMENT=Development`

3. **Docker:** Variáveis de ambiente no `docker-compose.yml`
   - Sobrescreve tudo com: `Server=sqlserver;Database=Be3DB;User Id=sa;Password=...`
   - Prioridade: Variáveis de ambiente > appsettings.Development.json > appsettings.json

### Frontend

1. **Desenvolvimento local:** `src/environments/environment.ts`
   - `apiUrl: 'http://localhost:5123/api'`
   - Usado quando roda `ng serve` ou `npm start`

2. **Docker/Produção:** `src/environments/environment.prod.ts`
   - `apiUrl: '/api'` (URL relativa)
   - Usado quando build é feito com `--configuration production`
   - O nginx faz proxy de `/api` para `http://backend:8080`

3. **Nginx no Docker:**
   - Serve os arquivos estáticos do Angular
   - Faz proxy reverso de `/api/*` para `http://backend:8080/*`
   - Permite que o frontend acesse o backend sem problemas de CORS

### CORS

O backend está configurado para aceitar requisições de:
- `http://localhost:4200` (desenvolvimento local)
- `http://frontend:80` (Docker)

## 🛠️ Troubleshooting

### Problema: Backend local não conecta ao banco

**Solução:**
1. Verifique se o SQL Server está rodando
2. Ajuste a connection string em `appsettings.Development.json`
3. Execute os scripts SQL para criar o banco

### Problema: Frontend local não acessa o backend

**Solução:**
1. Verifique se o backend está rodando em http://localhost:5123
2. Verifique o arquivo `environment.ts` - deve apontar para `http://localhost:5123/api`
3. Verifique o console do navegador para erros de CORS

### Problema: Docker não inicia

**Solução:**
1. Verifique se o Docker Desktop está rodando
2. Verifique os logs: `docker-compose logs`
3. Tente reconstruir: `docker-compose build --no-cache`

### Problema: Frontend no Docker não acessa o backend

**Solução:**
1. Verifique se ambos os containers estão rodando: `docker-compose ps`
2. Verifique os logs do nginx: `docker-compose logs frontend`
3. Acesse diretamente o backend: http://localhost:5123/swagger

## 📝 Notas Importantes

- **Não misture ambientes:** Se estiver rodando localmente, não inicie o Docker ao mesmo tempo (conflito de portas)
- **Banco de dados:** O banco local e o do Docker são independentes
- **Hot reload:** No ambiente local você tem hot reload. No Docker, precisa reconstruir a imagem após mudanças
- **Variáveis de ambiente:** No Docker, as variáveis de ambiente têm prioridade sobre os arquivos de configuração

## 🔄 Migrando entre Ambientes

### De Local para Docker

1. Pare os serviços locais (Ctrl+C)
2. Execute: `docker-compose up -d`
3. Aguarde a inicialização completa

### De Docker para Local

1. Pare o Docker: `docker-compose down`
2. Configure o banco local (scripts SQL)
3. Ajuste `appsettings.Development.json` se necessário
4. Inicie backend e frontend localmente

