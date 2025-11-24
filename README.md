# Be3 - Sistema de Cadastro de Pacientes

Sistema completo para cadastro e gerenciamento de pacientes desenvolvido com .NET 8 e Angular 18, seguindo princípios SOLID e arquitetura em camadas.

## 📋 Índice

- [Descrição do Projeto](#descrição-do-projeto)
- [Arquitetura](#arquitetura)
- [Tecnologias](#tecnologias)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Configuração](#instalação-e-configuração)
- [Executando a Aplicação](#executando-a-aplicação)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [API Endpoints](#api-endpoints)
- [Validações](#validações)
- [Funcionalidades](#funcionalidades)
- [Princípios SOLID](#princípios-solid)
- [Troubleshooting](#troubleshooting)

## 📖 Descrição do Projeto

Aplicação completa para cadastro e edição de pacientes com validações de negócio robustas, exclusão lógica e interface responsiva. O sistema permite:

- ✅ Cadastrar novos pacientes com todos os dados necessários
- ✅ Editar pacientes existentes
- ✅ Listar todos os pacientes (ativos e inativos)
- ✅ Ativar/Desativar pacientes (exclusão lógica com possibilidade de reativação)
- ✅ Validações em tempo real no frontend e backend
- ✅ Interface responsiva e intuitiva
- ✅ Feedback visual claro para todas as operações

## 🏗 Arquitetura

A aplicação segue os princípios da **Clean Architecture** e **SOLID**, organizando o código em camadas bem definidas, cada uma com uma responsabilidade específica.

### Camadas da Aplicação

#### Backend (.NET 8)

A arquitetura do backend é dividida em 4 camadas principais, cada uma seguindo o princípio da responsabilidade única:

**1. Be3.Domain (Camada de Domínio)**
- Esta é a camada mais interna, onde vivem as entidades do negócio
- Contém os modelos de domínio (Paciente, Convenio, Genero, Uf) e suas interfaces
- Não possui dependências externas - é puro e isolado
- Responsável por definir **O QUE** o sistema é, não **COMO** funciona
- Define contratos através de interfaces (IPacienteRepository, IConvenioRepository)

**2. Be3.Infrastructure (Camada de Infraestrutura)**
- Implementa o acesso a dados usando Dapper
- Responsável por **COMO** os dados são persistidos e recuperados
- Contém repositórios concretos que implementam as interfaces definidas no Domain
- Organiza queries SQL de forma limpa e manutenível em classes estáticas (PacienteQueries, ConvenioQueries)
- Gerencia conexões com o banco de dados através do DapperContext

**3. Be3.Application (Camada de Aplicação)**
- Contém a lógica de negócio da aplicação
- Transforma dados de entrada (DTOs) em entidades de domínio
- Realiza validações de negócio centralizadas através do PacienteValidator
- Usa AutoMapper para conversões entre camadas
- Contém services que orquestram as operações de negócio
- Define DTOs (Data Transfer Objects) para comunicação com a API

**4. Be3.Api (Camada de Apresentação)**
- Camada mais externa, expõe a API REST
- Controllers recebem requisições HTTP e delegam para a camada de aplicação
- Configura CORS, Swagger e injeção de dependências
- Trata erros HTTP e retorna respostas apropriadas
- Configuração de JSON serialization (CamelCase)

#### Frontend (Angular 18)

O frontend utiliza arquitetura baseada em componentes standalone:

- **Components**: Componentes isolados e reutilizáveis (standalone)
  - `PacienteListComponent`: Lista de pacientes com funcionalidade de ativar/desativar
  - `PacienteFormComponent`: Formulário de criação/edição
  - `NotificationComponent`: Sistema de notificações toast
- **Services**: Comunicação HTTP com a API backend
  - `PacienteService`: Operações CRUD de pacientes
  - `ConvenioService`: Listagem de convênios
  - `NotificationService`: Gerenciamento de notificações
- **Models**: Interfaces e tipos TypeScript para tipagem forte
- **Routes**: Configuração de roteamento lazy-loaded

### Analogia Bíblica: A Construção do Templo de Salomão

Assim como o Templo de Salomão foi construído em camadas bem definidas (fundação, paredes, teto, decoração), nossa aplicação também:

- **Domain** = A pedra fundamental - a base sólida e imutável sobre a qual tudo é construído. Assim como os fundamentos do templo foram estabelecidos primeiro e não mudavam, o Domain define os conceitos essenciais do negócio que permanecem constantes.

- **Infrastructure** = Os muros - fornecem estrutura e proteção, conectando o interior ao exterior. Assim como os muros protegiam o templo e permitiam que pessoas entrassem por portões específicos, a Infrastructure conecta nossa aplicação ao banco de dados de forma segura e organizada.

- **Application** = Os espaços internos - onde a vida acontece, a lógica de negócio reside. Assim como os salões do templo eram onde as atividades sagradas ocorriam, a Application é onde as regras de negócio são executadas, transformando dados e validando operações.

- **API** = A fachada e as portas - a interface com o mundo exterior, permitindo que pessoas entrem e interajam. Assim como a fachada impressionante do templo convidava pessoas a entrar, nossa API é a interface amigável que permite que clientes (frontend) interajam com o sistema.

Cada camada tem seu propósito específico e não interfere no trabalho das outras, assim como cada parte do templo tinha sua função sem comprometer a estrutura geral.

## 🛠 Tecnologias

### Backend
- **.NET 8.0** - Framework principal
- **Dapper** - ORM leve para acesso a dados (queries SQL organizadas)
- **SQL Server** - Banco de dados relacional
- **AutoMapper** - Mapeamento objeto-objeto entre DTOs e entidades
- **Swagger/OpenAPI** - Documentação interativa da API
- **Microsoft.Data.SqlClient** - Cliente SQL Server

### Frontend
- **Angular 18** - Framework SPA
- **TypeScript** - Superset do JavaScript com tipagem estática
- **ngx-mask** - Máscaras para campos de formulário (CPF, telefone)
- **Reactive Forms** - Formulários reativos com validação em tempo real
- **RxJS** - Programação reativa para requisições HTTP

### Banco de Dados
- **SQL Server** - Banco de dados relacional com constraints e índices

## 📦 Pré-requisitos

### Para Executar com Docker (Recomendado)

- [Docker Desktop](https://www.docker.com/products/docker-desktop) instalado e rodando
- Docker Compose (geralmente incluído no Docker Desktop)

**Vantagens:** Não precisa instalar .NET, Node.js ou SQL Server localmente. Tudo roda em containers isolados.

### Para Executar Localmente

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
- [Node.js](https://nodejs.org/) (versão 18 ou superior)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli@18`)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (ou SQL Server Express)
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms) (opcional, mas recomendado)

## ⚙️ Instalação e Configuração

### 1. Clonar o Repositório

```bash
git clone <url-do-repositorio>
cd entrevista2
```

### 2. Escolher Modo de Execução

#### 🐳 Modo Docker (Recomendado)

Se você escolheu executar com Docker, pule para a seção [Executar com Docker](#-opção-1-executar-com-docker-recomendado). O banco de dados será configurado automaticamente.

#### 💻 Modo Local

Se você escolheu executar localmente, continue com a configuração do banco de dados abaixo.

### 3. Configurar o Banco de Dados (Apenas para Modo Local)

#### Passo 1: Criar o Banco de Dados

Execute os scripts SQL na seguinte ordem usando o SQL Server Management Studio ou sqlcmd:

**Opção A: Via SSMS (SQL Server Management Studio)**
1. Abra o SSMS e conecte ao seu SQL Server
2. Abra e execute cada script na ordem:
   - `database\scripts\01_create_database.sql` - Cria o banco de dados
   - `database\scripts\02_create_tables.sql` - Cria as tabelas
   - `database\scripts\03_seed_convenios.sql` - Insere convênios de exemplo
   - `database\scripts\04_seed_pacientes.sql` - Insere pacientes de exemplo (opcional)

**Opção B: Via linha de comando (sqlcmd)**

```bash
# Para SQL Server Express local
sqlcmd -S localhost\SQLEXPRESS -E -i database\scripts\01_create_database.sql
sqlcmd -S localhost\SQLEXPRESS -E -i database\scripts\02_create_tables.sql
sqlcmd -S localhost\SQLEXPRESS -E -i database\scripts\03_seed_convenios.sql
sqlcmd -S localhost\SQLEXPRESS -E -i database\scripts\04_seed_pacientes.sql
```

#### Passo 2: Configurar Connection String

Edite o arquivo `backend\Be3.Api\appsettings.json` e atualize a connection string conforme seu ambiente:

**Para SQL Server Express:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=Be3DB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;"
  }
}
```

**Para SQL Server com autenticação SQL:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Be3DB;User Id=sa;Password=SuaSenhaAqui;TrustServerCertificate=True;"
  }
}
```

**Nota:** Ajuste o nome do servidor (`localhost\\SQLEXPRESS`) conforme sua instalação do SQL Server.

### 4. Restaurar Dependências do Backend (Apenas para Modo Local)

```bash
cd backend
dotnet restore
```

### 5. Restaurar Dependências do Frontend (Apenas para Modo Local)

```bash
cd frontend/be3-frontend
npm install
```

## 🚀 Executando a Aplicação

Este projeto pode ser executado de duas formas: **localmente** (desenvolvimento) ou **no Docker** (containerizado). Escolha a opção que melhor se adequa ao seu ambiente.

### 🐳 Opção 1: Executar com Docker (Recomendado)

A forma mais simples de executar o projeto é usando Docker. Todos os serviços (SQL Server, Backend e Frontend) são iniciados automaticamente.

#### Pré-requisitos Docker

- [Docker Desktop](https://www.docker.com/products/docker-desktop) instalado e rodando

#### Executar no Docker

```bash
# Na raiz do projeto
docker-compose up -d
```

Este comando irá:
- ✅ Baixar as imagens necessárias (se não estiverem em cache)
- ✅ Construir as imagens do backend e frontend
- ✅ Iniciar o SQL Server em container
- ✅ Executar os scripts SQL de inicialização do banco automaticamente
- ✅ Iniciar o backend e frontend

#### Acessar a Aplicação

Após alguns segundos, acesse:
- **Frontend:** http://localhost:4200
- **Backend API:** http://localhost:5123
- **Swagger:** http://localhost:5123/swagger
- **SQL Server:** localhost:1433

#### Comandos Úteis Docker

```bash
# Ver logs de todos os serviços
docker-compose logs -f

# Ver logs de um serviço específico
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f sqlserver

# Parar os serviços
docker-compose down

# Parar e remover volumes (limpar banco de dados)
docker-compose down -v

# Reconstruir as imagens
docker-compose build --no-cache
docker-compose up -d
```

#### Credenciais do Banco (Docker)

- **Servidor:** localhost,1433 (ou `sqlserver` dentro da rede Docker)
- **Usuário:** sa
- **Senha:** Be3@Password123!
- **Database:** Be3DB

📖 **Para mais detalhes sobre Docker, consulte:** [README.DOCKER.md](README.DOCKER.md)

---

### 💻 Opção 2: Executar Localmente (Desenvolvimento)

Para desenvolvimento local, você precisa ter .NET, Node.js e SQL Server instalados.

#### Executar Backend

1. Abra um terminal na pasta `backend`
2. Execute:

```bash
dotnet run --project Be3.Api/Be3.Api.csproj
```

Ou se estiver na pasta raiz:

```bash
cd backend/Be3.Api
dotnet run
```

O backend estará disponível em:
- **API HTTP:** `http://localhost:5123`
- **API HTTPS:** `https://localhost:7116` (se configurado)
- **Swagger:** `http://localhost:5123/swagger`

**Nota:** A porta pode variar. Verifique a saída do terminal para a URL exata.

#### Executar Frontend

1. Abra um **novo terminal** na pasta `frontend/be3-frontend`
2. Execute:

```bash
ng serve
```

Ou:

```bash
npm start
```

O frontend estará disponível em:
- **Aplicação:** `http://localhost:4200`

**Nota:** O frontend está configurado para se comunicar com o backend em `http://localhost:5123`. Se a porta do backend for diferente, atualize o arquivo `frontend/be3-frontend/src/environments/environment.ts`.

#### Acessar a Aplicação

Abra seu navegador e acesse: `http://localhost:4200`

📖 **Para mais detalhes sobre configuração local vs Docker, consulte:** [README.AMBIENTES.md](README.AMBIENTES.md)

## 📁 Estrutura do Projeto

```
entrevista2/
├── backend/
│   ├── Be3.Api/                    # Camada de apresentação
│   │   ├── Controllers/            # Controllers REST API
│   │   │   ├── PacientesController.cs
│   │   │   └── ConveniosController.cs
│   │   ├── Program.cs              # Configuração da aplicação
│   │   └── appsettings.json        # Configurações (connection string)
│   ├── Be3.Application/            # Camada de aplicação
│   │   ├── Services/               # Serviços de negócio
│   │   │   ├── PacienteService.cs
│   │   │   └── ConvenioService.cs
│   │   ├── DTOs/                   # Data Transfer Objects
│   │   │   ├── PacienteDTO.cs
│   │   │   ├── CreatePacienteDTO.cs
│   │   │   └── UpdatePacienteDTO.cs
│   │   ├── Validators/             # Validadores de negócio
│   │   │   └── PacienteValidator.cs
│   │   └── Mappings/               # AutoMapper profiles
│   │       └── MappingProfile.cs
│   ├── Be3.Domain/                 # Camada de domínio
│   │   ├── Models/                 # Entidades de domínio
│   │   │   ├── Paciente.cs
│   │   │   ├── Convenio.cs
│   │   │   ├── Genero.cs
│   │   │   └── Uf.cs
│   │   └── Repositories/           # Interfaces de repositório
│   │       ├── IPacienteRepository.cs
│   │       └── IConvenioRepository.cs
│   ├── Be3.Infrastructure/         # Camada de infraestrutura
│   │   ├── Repositories/           # Implementações de repositório
│   │   │   ├── PacienteRepository.cs
│   │   │   └── ConvenioRepository.cs
│   │   ├── Queries/                # Queries SQL organizadas
│   │   │   ├── PacienteQueries.cs
│   │   │   └── ConvenioQueries.cs
│   │   └── DapperContext.cs        # Contexto de conexão
│   └── Be3.sln                     # Solution file
├── frontend/
│   └── be3-frontend/
│       ├── src/
│       │   ├── app/
│       │   │   ├── components/     # Componentes Angular
│       │   │   │   ├── paciente-list/
│       │   │   │   ├── paciente-list.component.ts
│       │   │   │   ├── paciente-list.html
│       │   │   │   └── paciente-list.css
│       │   │   │   ├── paciente-form/
│       │   │   │   └── ...
│       │   │   │   └── notification/
│       │   │   │       └── ...
│       │   │   ├── services/       # Serviços HTTP
│       │   │   │   ├── paciente.service.ts
│       │   │   │   ├── convenio.service.ts
│       │   │   │   └── notification.service.ts
│       │   │   ├── models/         # Interfaces TypeScript
│       │   │   │   └── paciente.model.ts
│       │   │   ├── app.routes.ts   # Rotas
│       │   │   └── app.config.ts   # Configuração
│       │   └── ...
│       └── package.json
├── database/
│   └── scripts/
│       ├── 01_create_database.sql  # Criação do banco
│       ├── 02_create_tables.sql    # Criação das tabelas
│       ├── 03_seed_convenios.sql   # Dados de convênios
│       └── 04_seed_pacientes.sql   # Dados de pacientes (opcional)
└── README.md
```

## 🔌 API Endpoints

### Pacientes

| Método | Endpoint | Descrição | Body |
|--------|----------|-----------|------|
| `GET` | `/api/pacientes` | Lista todos os pacientes | - |
| `GET` | `/api/pacientes/{id}` | Busca paciente por ID | - |
| `POST` | `/api/pacientes` | Cria novo paciente | `CreatePacienteDTO` |
| `PUT` | `/api/pacientes/{id}` | Atualiza paciente existente | `UpdatePacienteDTO` |
| `DELETE` | `/api/pacientes/{id}` | Desativa paciente (exclusão lógica) | - |
| `PATCH` | `/api/pacientes/{id}/ativar` | Reativa paciente desativado | - |

### Convênios

| Método | Endpoint | Descrição | Body |
|--------|----------|-----------|------|
| `GET` | `/api/convenios` | Lista todos os convênios ativos | - |
| `GET` | `/api/convenios/{id}` | Busca convênio por ID | - |

### Documentação Swagger

Acesse `http://localhost:5123/swagger` para visualizar a documentação interativa da API com exemplos de requisições e respostas.

### Exemplos de Requisições

**Criar Paciente:**
```json
POST /api/pacientes
Content-Type: application/json

{
  "nome": "João",
  "sobrenome": "Silva",
  "dataNascimento": "1990-05-15T00:00:00",
  "genero": 1,
  "cpf": "12345678901",
  "rg": "123456789",
  "ufRG": 25,
  "email": "joao.silva@email.com",
  "celular": "11987654321",
  "telefoneFixo": "1133334444",
  "convenioId": 1,
  "numeroCarteirinha": "123456",
  "validadeCarteirinha": "2025-12-01T00:00:00"
}
```

**Atualizar Paciente:**
```json
PUT /api/pacientes/1
Content-Type: application/json

{
  "nome": "João",
  "sobrenome": "Silva Santos",
  "dataNascimento": "1990-05-15T00:00:00",
  "genero": 1,
  "cpf": "12345678901",
  "rg": "123456789",
  "ufRG": 25,
  "email": "joao.silva@email.com",
  "celular": "11987654321"
}
```

## ✔️ Validações

### Backend

As validações de negócio são implementadas na camada Application através do `PacienteValidator`:

1. **CPF**: 
   - Se informado, deve ser válido (algoritmo de validação com dígitos verificadores)
   - Deve ser único no sistema (considerando apenas pacientes ativos)
   - Não é obrigatório

2. **Email**: 
   - Formato válido (regex de validação)
   - Campo obrigatório

3. **Telefones**: 
   - Pelo menos um (Celular OU Telefone Fixo) deve ser informado
   - Formato válido (10 ou 11 dígitos após remover caracteres especiais)

4. **Data de Nascimento**: 
   - Não pode ser futura
   - Não pode ter mais de 150 anos

5. **RG**: 
   - Campo obrigatório

6. **UfRG**: 
   - Deve ser um estado brasileiro válido (1-27)

7. **Convênio**: 
   - Se informado, deve existir na base de dados
   - Se informado o convênio, número da carteirinha é recomendado

### Frontend

Validações em tempo real usando Angular Reactive Forms:

- ✅ Validação síncrona de campos obrigatórios
- ✅ Validação de formato de email
- ✅ Validação customizada para telefones (pelo menos um)
- ✅ Máscaras visuais para CPF (`000.000.000-00`), telefone (`(00) 00000-0000`) e celular (`(00) 0000-0000`)
- ✅ Feedback visual imediato para o usuário
- ✅ Mensagens de erro contextuais
- ✅ Validação de data de nascimento (date picker HTML5)

### Banco de Dados

Constraints e validações no nível de banco:

- ✅ Constraint `UNIQUE` no CPF
- ✅ Constraint `CHECK` para garantir que pelo menos um telefone seja informado
- ✅ Constraint `CHECK` para data de nascimento não futura
- ✅ Foreign Key para convênio
- ✅ Índices para performance (CPF, Email, Ativo)

## ✨ Funcionalidades

### Gestão de Pacientes

- **Cadastro Completo**: Todos os campos necessários com validações
- **Edição**: Atualização de dados com preservação do histórico
- **Listagem**: Visualização de todos os pacientes com informações resumidas
- **Exclusão Lógica**: Pacientes nunca são fisicamente deletados, apenas marcados como inativos
- **Reativação**: Pacientes desativados podem ser reativados através do botão "Ativar"
- **Status Visual**: Badges coloridos indicando status (Ativo/Inativo)

### Interface do Usuário

- **Responsiva**: Interface adaptável a diferentes tamanhos de tela
- **Feedback Visual**: Notificações toast para sucesso e erro
- **Modal de Confirmação**: Confirmação antes de ativar/desativar pacientes
- **Loading States**: Indicadores de carregamento durante operações
- **Tratamento de Erros**: Mensagens claras e acionáveis

### Sistema de Notificações

- Notificações toast não intrusivas
- Tipos: Sucesso, Erro, Info, Aviso
- Auto-dismiss após 5 segundos
- Botão de fechamento manual

## 🎯 Princípios SOLID

A aplicação segue rigorosamente os princípios SOLID:

### S - Single Responsibility Principle (Princípio da Responsabilidade Única)

Cada classe tem uma única responsabilidade:

- `PacienteRepository`: apenas acesso a dados de pacientes (SELECT, INSERT, UPDATE)
- `PacienteService`: apenas lógica de negócio e orquestração
- `PacienteValidator`: apenas validações de regras de negócio
- `PacientesController`: apenas receber requisições HTTP e retornar respostas
- `PacienteQueries`: apenas armazenar queries SQL

### O - Open/Closed Principle (Princípio Aberto/Fechado)

O sistema está aberto para extensão e fechado para modificação:

- Novos tipos de validação podem ser adicionados ao `PacienteValidator` sem modificar código existente
- Novos repositórios podem ser criados implementando as interfaces do Domain
- Novos endpoints podem ser adicionados aos controllers sem modificar serviços existentes
- Novos campos podem ser adicionados através de DTOs sem alterar entidades de domínio

### L - Liskov Substitution Principle (Princípio da Substituição de Liskov)

Implementações concretas respeitam os contratos das interfaces:

- `PacienteRepository` e `ConvenioRepository` podem ser substituídos por suas interfaces (`IPacienteRepository`, `IConvenioRepository`) sem quebrar o código
- Qualquer implementação que respeite a interface pode ser usada no lugar da implementação padrão

### I - Interface Segregation Principle (Princípio da Segregação de Interface)

Interfaces específicas e coesas:

- `IPacienteRepository` contém apenas métodos relacionados a pacientes
- `IConvenioRepository` contém apenas métodos relacionados a convênios
- Interfaces não forçam implementação de métodos desnecessários

### D - Dependency Inversion Principle (Princípio da Inversão de Dependência)

Dependências apontam para abstrações, não para implementações concretas:

- `PacienteService` depende de `IPacienteRepository`, não de `PacienteRepository`
- `PacientesController` depende de `IPacienteService`, não de `PacienteService`
- Dependências são injetadas via construtor (Dependency Injection)
- Fácil de testar através de mocks das interfaces

## 📚 Documentação Adicional

- **[README.DOCKER.md](README.DOCKER.md)** - Guia completo sobre Docker, comandos úteis e troubleshooting
- **[README.AMBIENTES.md](README.AMBIENTES.md)** - Como configurar e usar o projeto em ambientes local e Docker
- **[CONFIGURAR_BANCO.md](CONFIGURAR_BANCO.md)** - Guia rápido para configurar o banco de dados localmente

## 🐛 Troubleshooting

### Problema: Erro de conexão com o banco de dados

**Sintomas:** `SqlException: Erro de rede ou específico à instância ao estabelecer conexão`

**Soluções:**
1. Verifique se o SQL Server está rodando:
   - Abra o "SQL Server Configuration Manager"
   - Verifique se o serviço "SQL Server (SQLEXPRESS)" está "Running"

2. Verifique a connection string em `appsettings.json`:
   - O nome do servidor está correto?
   - O nome do banco de dados existe?
   - A autenticação está configurada corretamente?

3. Teste a conexão:
   ```bash
   sqlcmd -S localhost\SQLEXPRESS -E
   ```

### Problema: CORS Error no navegador

**Sintomas:** `Access to XMLHttpRequest has been blocked by CORS policy`

**Solução:**
- Verifique se o backend está rodando
- Verifique se a porta do backend no `appsettings.json` corresponde à porta configurada no CORS do `Program.cs`
- Certifique-se de que `UseCors("AllowAngular")` está antes de `UseAuthorization()` no `Program.cs`

### Problema: Erro 400 (Bad Request) ao criar/atualizar paciente

**Sintomas:** `One or more validation errors occurred`

**Solução:**
- Verifique os dados enviados no console do navegador (F12)
- Confirme que todos os campos obrigatórios estão preenchidos
- Verifique o formato dos dados (data, CPF, telefone)

### Problema: Frontend não se conecta ao backend

**Sintomas:** `HttpErrorResponse: Http failure response`

**Soluções:**
1. Verifique se o backend está rodando na porta correta
2. **Modo Local:** Atualize a URL da API em `frontend/be3-frontend/src/environments/environment.ts`:
   ```typescript
   apiUrl: 'http://localhost:5123/api'
   ```
3. **Modo Docker:** O frontend usa proxy nginx. Verifique se o backend está rodando: `docker-compose ps`

### Problema: Erro de tracking no Angular (NG0955)

**Sintomas:** `The provided track expression resulted in duplicated keys`

**Solução:**
- Este erro foi corrigido usando `track $index` no template
- Se ainda ocorrer, verifique se há pacientes sem ID válido na lista

### Problema: Campo `ativo` sempre aparece como `true`

**Sintomas:** Status sempre mostra "Ativo" mesmo após desativar

**Soluções:**
1. Verifique se o backend está retornando o campo corretamente
2. Verifique se o JSON está em camelCase (configurado no `Program.cs`)
3. Limpe o cache do navegador e recarregue

## 📝 Notas Importantes

- **Exclusão Lógica**: Pacientes nunca são fisicamente deletados. O campo `Ativo` controla a visibilidade. Pacientes desativados podem ser reativados através do endpoint `PATCH /api/pacientes/{id}/ativar`

- **Validação Dupla**: Validações existem tanto no frontend (UX) quanto no backend (segurança). Nunca confie apenas nas validações do frontend

- **DTOs**: Objetos de transferência separam os modelos de domínio da API, permitindo evolução independente e proteção do domínio interno

- **Queries Organizadas**: SQL está organizado em classes estáticas (`PacienteQueries`, `ConvenioQueries`) para facilitar manutenção, testes e reutilização

- **JSON Naming**: Backend configurado para retornar JSON em camelCase (`ativo`, `nome`) para compatibilidade com convenções JavaScript/TypeScript

- **CORS**: Configurado para desenvolvimento local (`http://localhost:4200`) e Docker (`http://frontend:80`). Em produção, atualize o `Program.cs`

- **Ambientes**: O projeto está configurado para funcionar tanto localmente quanto no Docker. As configurações são selecionadas automaticamente conforme o ambiente de execução. Veja [README.AMBIENTES.md](README.AMBIENTES.md) para detalhes.

- **Docker**: Todos os serviços (SQL Server, Backend, Frontend) podem ser executados com um único comando: `docker-compose up -d`. Veja [README.DOCKER.md](README.DOCKER.md) para detalhes.

## 🤝 Contribuindo

Este é um projeto de teste técnico. Para melhorias ou correções:

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto foi desenvolvido como teste técnico para demonstrar conhecimentos em:
- Arquitetura de Software (Clean Architecture)
- Princípios SOLID
- Desenvolvimento Full-Stack (.NET + Angular)
- Boas práticas de desenvolvimento

## 👨‍💻 Autor

Desenvolvido seguindo princípios de Clean Architecture e SOLID para demonstrar conhecimento em arquitetura de software, separação de responsabilidades, testabilidade e boas práticas de desenvolvimento.

---

**Desenvolvido com dedicação e atenção aos detalhes, seguindo os princípios que fazem código de qualidade.** 🌟

*"Assim como a construção do templo exigiu planejamento, organização e atenção aos detalhes, este código foi construído com os mesmos valores - cada camada bem definida, cada responsabilidade claramente estabelecida, cada decisão arquitetural cuidadosamente pensada."*