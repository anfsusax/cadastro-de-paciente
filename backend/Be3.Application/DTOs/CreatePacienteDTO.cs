namespace Be3.Application.DTOs;

public class CreatePacienteDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public int Genero { get; set; }
    public string? CPF { get; set; }
    public string RG { get; set; } = string.Empty;
    public int UfRG { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Celular { get; set; }
    public string? TelefoneFixo { get; set; }
    public int? ConvenioId { get; set; }
    public string? NumeroCarteirinha { get; set; }
    public DateTime? ValidadeCarteirinha { get; set; }
}
