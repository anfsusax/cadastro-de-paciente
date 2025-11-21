# 🚀 Solução Rápida - Configuração do Banco Be3DB

## ✅ Status
- SQL Server Express está **RODANDO** ✅
- Connection string foi atualizada para usar autenticação do Windows
- Próximo passo: Criar o banco de dados

## 📝 Criar o Banco de Dados

Execute os scripts SQL nesta ordem:

### Opção 1: SQL Server Management Studio (SSMS)

1. Abra o **SQL Server Management Studio**
2. Conecte-se ao servidor: `localhost\SQLEXPRESS`
3. Execute cada script nesta ordem:
   - Abra e execute: `database\scripts\01_create_database.sql`
   - Depois: `database\scripts\02_create_tables.sql`
   - Depois: `database\scripts\03_seed_convenios.sql`
   - Opcional: `database\scripts\04_seed_pacientes.sql`

### Opção 2: Linha de Comando (sqlcmd)

Abra o PowerShell como Administrador e execute:

```powershell
cd G:\aulas\entrevistas\entrevista2

# Execute os scripts na ordem:
sqlcmd -S localhost\SQLEXPRESS -i database\scripts\01_create_database.sql
sqlcmd -S localhost\SQLEXPRESS -i database\scripts\02_create_tables.sql
sqlcmd -S localhost\SQLEXPRESS -i database\scripts\03_seed_convenios.sql
sqlcmd -S localhost\SQLEXPRESS -i database\scripts\04_seed_pacientes.sql
```

## 🔄 Reiniciar o Backend

Após criar o banco, **pare e reinicie o backend**:

1. Vá na janela do PowerShell do backend
2. Pressione `Ctrl+C` para parar
3. Execute novamente: `dotnet run`

## ✅ Verificar

Acesse no navegador:
- **Swagger:** http://localhost:5123/swagger
- Teste: GET `/api/convenios` - deve retornar a lista de convênios

Se funcionar, está tudo configurado! ✅
