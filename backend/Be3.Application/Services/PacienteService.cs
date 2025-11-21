using AutoMapper;
using Be3.Application.DTOs;
using Be3.Application.Validators;
using Be3.Domain.Models;
using Be3.Domain.Repositories;

namespace Be3.Application.Services;

public class PacienteService : IPacienteService
{
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IMapper _mapper;
    private readonly PacienteValidator _validator;

    public PacienteService(
        IPacienteRepository pacienteRepository,
        IMapper mapper,
        PacienteValidator validator)
    {
        _pacienteRepository = pacienteRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<IEnumerable<PacienteDTO>> ObterTodosAsync()
    {
        var pacientes = await _pacienteRepository.ObterTodosAsync();
        return _mapper.Map<IEnumerable<PacienteDTO>>(pacientes);
    }

    public async Task<PacienteDTO?> ObterPorIdAsync(int id)
    {
        var paciente = await _pacienteRepository.ObterPorIdAsync(id);
        return paciente == null ? null : _mapper.Map<PacienteDTO>(paciente);
    }

    public async Task<PacienteDTO> CriarAsync(CreatePacienteDTO dto)
    {
        var resultadoValidacao = await _validator.ValidarCriarAsync(dto);
        if (!resultadoValidacao.IsValid)
        {
            throw new ValidationException(resultadoValidacao.Erros);
        }

        var paciente = _mapper.Map<Paciente>(dto);
        paciente = await _pacienteRepository.CriarAsync(paciente);
        
        var pacienteCompleto = await _pacienteRepository.ObterPorIdAsync(paciente.Id);
        return _mapper.Map<PacienteDTO>(pacienteCompleto!);
    }

    public async Task<PacienteDTO> AtualizarAsync(int id, UpdatePacienteDTO dto)
    {
        var pacienteExistente = await _pacienteRepository.ObterPorIdAsync(id);
        if (pacienteExistente == null)
        {
            throw new NotFoundException($"Paciente com ID {id} não encontrado.");
        }

        var resultadoValidacao = await _validator.ValidarAtualizarAsync(id, dto);
        if (!resultadoValidacao.IsValid)
        {
            throw new ValidationException(resultadoValidacao.Erros);
        }

        _mapper.Map(dto, pacienteExistente);
        await _pacienteRepository.AtualizarAsync(pacienteExistente);

        var pacienteAtualizado = await _pacienteRepository.ObterPorIdAsync(id);
        return _mapper.Map<PacienteDTO>(pacienteAtualizado!);
    }

    public async Task DesativarAsync(int id)
    {
        var paciente = await _pacienteRepository.ObterPorIdAsync(id);
        if (paciente == null)
        {
            throw new NotFoundException($"Paciente com ID {id} não encontrado.");
        }

        await _pacienteRepository.DesativarAsync(id);
    }

    public async Task AtivarAsync(int id)
    {
        var paciente = await _pacienteRepository.ObterPorIdAsync(id);
        if (paciente == null)
        {
            throw new NotFoundException($"Paciente com ID {id} não encontrado.");
        }

        await _pacienteRepository.AtivarAsync(id);
    }
}

public class ValidationException : Exception
{
    public IReadOnlyList<ValidationError> Erros { get; }

    public ValidationException(IReadOnlyList<ValidationError> erros)
        : base("Erro de validação")
    {
        Erros = erros;
    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
