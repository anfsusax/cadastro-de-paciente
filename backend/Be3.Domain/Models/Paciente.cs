namespace Be3.Domain.Models;

public class Paciente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public Genero Genero { get; set; }
    public string? CPF { get; set; }
    public string RG { get; set; } = string.Empty;
    public Uf UfRG { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Celular { get; set; }
    public string? TelefoneFixo { get; set; }
    public int? ConvenioId { get; set; }
    public string? NumeroCarteirinha { get; set; }
    public DateTime? ValidadeCarteirinha { get; set; }
    public bool Ativo { get; set; } = true;
    
    public Convenio? Convenio { get; set; }
}
