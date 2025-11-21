using Be3.Application.DTOs;

namespace Be3.Application.Services;

public interface IPacienteService
{
    Task<IEnumerable<PacienteDTO>> ObterTodosAsync();
    Task<PacienteDTO?> ObterPorIdAsync(int id);
    Task<PacienteDTO> CriarAsync(CreatePacienteDTO dto);
    Task<PacienteDTO> AtualizarAsync(int id, UpdatePacienteDTO dto);
    Task DesativarAsync(int id);
    Task AtivarAsync(int id);
}
