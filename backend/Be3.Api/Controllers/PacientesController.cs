using Be3.Application.DTOs;
using Be3.Application.Services;
using Be3.Application.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Be3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;

    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PacienteDTO>>> ObterTodos()
    {
        var pacientes = await _pacienteService.ObterTodosAsync();
        return Ok(pacientes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PacienteDTO>> ObterPorId(int id)
    {
        var paciente = await _pacienteService.ObterPorIdAsync(id);
        if (paciente == null)
        {
            return NotFound();
        }
        return Ok(paciente);
    }

    [HttpPost]
    public async Task<ActionResult<PacienteDTO>> Criar([FromBody] CreatePacienteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => new ValidationError(x.Key, e.ErrorMessage)))
                .ToList();
            return BadRequest(new { erros = errors });
        }

        try
        {
            var paciente = await _pacienteService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = paciente.Id }, paciente);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { erros = ex.Erros });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor", detalhes = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PacienteDTO>> Atualizar(int id, [FromBody] UpdatePacienteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => new ValidationError(x.Key, e.ErrorMessage)))
                .ToList();
            return BadRequest(new { erros = errors });
        }

        try
        {
            var paciente = await _pacienteService.AtualizarAsync(id, dto);
            return Ok(paciente);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { erros = ex.Erros });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor", detalhes = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Desativar(int id)
    {
        try
        {
            await _pacienteService.DesativarAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    [HttpPatch("{id}/ativar")]
    public async Task<IActionResult> Ativar(int id)
    {
        try
        {
            await _pacienteService.AtivarAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
}
