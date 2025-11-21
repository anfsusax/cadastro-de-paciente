using Be3.Domain.Models;
using Be3.Domain.Repositories;
using Be3.Infrastructure.Queries;
using Dapper;

namespace Be3.Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly DapperContext _context;

    public PacienteRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Paciente>> ObterTodosAsync()
    {
        using var connection = _context.CreateConnection();
        var pacientes = await connection.QueryAsync<Paciente, Convenio, Paciente>(
            PacienteQueries.ObterTodos,
            (paciente, convenio) =>
            {
                paciente.Convenio = convenio;
                return paciente;
            },
            splitOn: "Id"
        );
        return pacientes;
    }

    public async Task<Paciente?> ObterPorIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        var pacientes = await connection.QueryAsync<Paciente, Convenio, Paciente>(
            PacienteQueries.ObterPorId,
            (paciente, convenio) =>
            {
                paciente.Convenio = convenio;
                return paciente;
            },
            new { Id = id },
            splitOn: "Id"
        );
        return pacientes.FirstOrDefault();
    }

    public async Task<Paciente?> ObterPorCpfAsync(string cpf)
    {
        using var connection = _context.CreateConnection();
        var pacientes = await connection.QueryAsync<Paciente, Convenio, Paciente>(
            PacienteQueries.ObterPorCpf,
            (paciente, convenio) =>
            {
                paciente.Convenio = convenio;
                return paciente;
            },
            new { CPF = cpf },
            splitOn: "Id"
        );
        return pacientes.FirstOrDefault();
    }

    public async Task<Paciente> CriarAsync(Paciente paciente)
    {
        using var connection = _context.CreateConnection();
        var id = await connection.QuerySingleAsync<int>(PacienteQueries.Inserir, new
        {
            paciente.Nome,
            paciente.Sobrenome,
            paciente.DataNascimento,
            Genero = (int)paciente.Genero,
            paciente.CPF,
            paciente.RG,
            UfRG = (int)paciente.UfRG,
            paciente.Email,
            paciente.Celular,
            paciente.TelefoneFixo,
            paciente.ConvenioId,
            paciente.NumeroCarteirinha,
            paciente.ValidadeCarteirinha,
            paciente.Ativo
        });
        paciente.Id = id;
        return paciente;
    }

    public async Task<Paciente> AtualizarAsync(Paciente paciente)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(PacienteQueries.Atualizar, new
        {
            paciente.Id,
            paciente.Nome,
            paciente.Sobrenome,
            paciente.DataNascimento,
            Genero = (int)paciente.Genero,
            paciente.CPF,
            paciente.RG,
            UfRG = (int)paciente.UfRG,
            paciente.Email,
            paciente.Celular,
            paciente.TelefoneFixo,
            paciente.ConvenioId,
            paciente.NumeroCarteirinha,
            paciente.ValidadeCarteirinha
        });
        return paciente;
    }

    public async Task<bool> ExisteCpfAsync(string cpf, int? excludeId = null)
    {
        using var connection = _context.CreateConnection();
        var count = await connection.QuerySingleAsync<int>(PacienteQueries.ExisteCpf, new
        {
            CPF = cpf,
            ExcludeId = excludeId
        });
        return count > 0;
    }

    public async Task DesativarAsync(int id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(PacienteQueries.Desativar, new { Id = id });
    }

    public async Task AtivarAsync(int id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(PacienteQueries.Ativar, new { Id = id });
    }
}
