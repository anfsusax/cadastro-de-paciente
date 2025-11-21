using Be3.Domain.Models;

namespace Be3.Domain.Repositories;

public interface IPacienteRepository
{
    Task<IEnumerable<Paciente>> ObterTodosAsync();
    Task<Paciente?> ObterPorIdAsync(int id);
    Task<Paciente?> ObterPorCpfAsync(string cpf);
    Task<Paciente> CriarAsync(Paciente paciente);
    Task<Paciente> AtualizarAsync(Paciente paciente);
    Task<bool> ExisteCpfAsync(string cpf, int? excludeId = null);
    Task AtivarAsync(int id);
    Task DesativarAsync(int id);
}
