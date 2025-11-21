# 🔧 Configuração Rápida do Banco de Dados - Be3

## ⚠️ Problema Atual
O backend está tentando conectar ao SQL Server, mas não consegue encontrar o servidor.

## ✅ Solução Rápida

### Opção 1: SQL Server Local com Autenticação do Windows (RECOMENDADO)

1. **Edite o arquivo:** `backend\Be3.Api\appsettings.json`

2. **Substitua a connection string por:**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Be3DB;Integrated Security=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

3. **Se usar SQL Server Express, use:**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=Be3DB;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

### Opção 2: Verificar Serviços do SQL Server

1. Pressione `Win + R`
2. Digite: `services.msc`
3. Procure por serviços com nome `SQL Server`
4. Verifique se estão **Iniciados**
5. Se não estiverem, clique com botão direito → **Iniciar**

### Opção 3: Instalar SQL Server Express (se não tiver instalado)

1. Baixe o SQL Server Express: https://www.microsoft.com/sql-server/sql-server-downloads
2. Instale seguindo o assistente
3. Use a connection string da Opção 1 acima

## 📝 Criar o Banco de Dados

Após configurar a connection string, execute os scripts SQL na ordem:

### Via SQL Server Management Studio (SSMS):

1. Abra o **SQL Server Management Studio**
2. Conecte-se ao servidor (geralmente `localhost` ou `localhost\SQLEXPRESS`)
3. Execute cada script nesta ordem:
   - `database\scripts\01_create_database.sql`
   - `database\scripts\02_create_tables.sql`
   - `database\scripts\03_seed_convenios.sql`
   - `database\scripts\04_seed_pacientes.sql` (opcional - dados de teste)

### Via Linha de Comando (sqlcmd):

```powershell
# Navegue até a pasta do projeto
cd G:\aulas\entrevistas\entrevista2

# Execute os scripts (ajuste o servidor se necessário)
sqlcmd -S localhost -i database\scripts\01_create_database.sql
sqlcmd -S localhost -i database\scripts\02_create_tables.sql
sqlcmd -S localhost -i database\scripts\03_seed_convenios.sql
sqlcmd -S localhost -i database\scripts\04_seed_pacientes.sql
```

**Se usar SQL Server Express:**
```powershell
sqlcmd -S localhost\SQLEXPRESS -i database\scripts\01_create_database.sql
# ... e assim por diante
```

## 🔄 Reiniciar o Backend

Após configurar o banco de dados:

1. Pare o backend (Ctrl+C na janela do PowerShell)
2. Reinicie:
   ```powershell
   cd backend\Be3.Api
   dotnet run
   ```

## ✅ Verificar se Funcionou

Acesse no navegador:
- **Swagger:** http://localhost:5123/swagger
- Teste o endpoint: **GET /api/convenios**

Se retornar a lista de convênios, está tudo configurado! ✅

## 🆘 Problemas Comuns

### Erro: "Cannot open database"
**Solução:** Execute o script `01_create_database.sql` primeiro

### Erro: "Login failed"
**Solução:** Use `Integrated Security=True` na connection string (autenticação do Windows)

### Erro: "Server not found"
**Solução:** 
- Verifique se o SQL Server está rodando (services.msc)
- Tente `localhost\SQLEXPRESS` se usar Express
- Verifique se o SQL Server está instalado

### Não tenho SQL Server instalado
**Soluções:**
1. Instale SQL Server Express (gratuito)
2. Ou use Docker para rodar SQL Server em container (veja README.md)

## 📞 Próximos Passos

Depois de configurar o banco, a aplicação deve funcionar completamente:
- ✅ Listar pacientes
- ✅ Criar pacientes
- ✅ Editar pacientes
- ✅ Desativar pacientes
