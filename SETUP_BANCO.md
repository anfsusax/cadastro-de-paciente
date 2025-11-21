# Configuração do Banco de Dados - Be3

## ⚠️ Importante

O banco de dados precisa ser criado antes de usar a aplicação. Siga os passos abaixo:

## Passo 1: Verificar SQL Server

Certifique-se de que o SQL Server está instalado e rodando:

1. Abra o **SQL Server Configuration Manager**
2. Verifique se o serviço **SQL Server (MSSQLSERVER)** ou **SQL Server (SQLEXPRESS)** está **Iniciado**

## Passo 2: Criar o Banco de Dados

### Opção A: Usando SQL Server Management Studio (SSMS)

1. Abra o **SQL Server Management Studio**
2. Conecte-se ao servidor (geralmente `localhost` ou `localhost\SQLEXPRESS`)
3. Abra cada script SQL em ordem e execute:

```sql
-- 1. Execute primeiro: database/scripts/01_create_database.sql
-- 2. Execute segundo: database/scripts/02_create_tables.sql  
-- 3. Execute terceiro: database/scripts/03_seed_convenios.sql
```

### Opção B: Usando sqlcmd (Linha de Comando)

Abra o PowerShell como Administrador e execute:

```powershell
# Execute na ordem:
sqlcmd -S localhost -i database\scripts\01_create_database.sql
sqlcmd -S localhost -i database\scripts\02_create_tables.sql
sqlcmd -S localhost -i database\scripts\03_seed_convenios.sql
```

**Nota:** Se usar SQL Server Express, altere `localhost` para `localhost\SQLEXPRESS`

## Passo 3: Configurar Connection String

Edite o arquivo `backend\Be3.Api\appsettings.json` e ajuste a connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Be3DB;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True;"
  }
}
```

**Opções de Connection String:**

- **SQL Server Local:**
  ```
  Server=localhost;Database=Be3DB;Integrated Security=True;TrustServerCertificate=True;
  ```

- **SQL Server com Autenticação SQL:**
  ```
  Server=localhost;Database=Be3DB;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True;
  ```

- **SQL Server Express:**
  ```
  Server=localhost\SQLEXPRESS;Database=Be3DB;Integrated Security=True;TrustServerCertificate=True;
  ```

## Passo 4: Reiniciar o Backend

Após configurar o banco de dados:

1. Pare o backend (Ctrl+C no terminal onde está rodando)
2. Execute novamente: `cd backend\Be3.Api && dotnet run`

## Verificar se Funcionou

Acesse no navegador:
- **Swagger:** http://localhost:5123/swagger
- Teste o endpoint: GET `/api/convenios` - deve retornar a lista de convênios

Se retornar dados, o banco está configurado corretamente! ✅
