using Be3.Application.DTOs;
using Be3.Domain.Models;
using Be3.Domain.Repositories;

namespace Be3.Application.Validators;

public class PacienteValidator
{
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IConvenioRepository _convenioRepository;

    public PacienteValidator(
        IPacienteRepository pacienteRepository,
        IConvenioRepository convenioRepository)
    {
        _pacienteRepository = pacienteRepository;
        _convenioRepository = convenioRepository;
    }

    public async Task<ValidationResult> ValidarCriarAsync(CreatePacienteDTO dto)
    {
        var resultado = new ValidationResult();

        ValidarCamposObrigatorios(dto, resultado);
        ValidarEmail(dto.Email, resultado);
        ValidarTelefones(dto.Celular, dto.TelefoneFixo, resultado);
        ValidarDataNascimento(dto.DataNascimento, resultado);
        ValidarUfRG(dto.UfRG, resultado);

        if (!string.IsNullOrWhiteSpace(dto.CPF))
        {
            ValidarCpf(dto.CPF, resultado);
            var existeCpf = await _pacienteRepository.ExisteCpfAsync(dto.CPF);
            if (existeCpf)
            {
                resultado.AdicionarErro("CPF", "CPF já cadastrado no sistema.");
            }
        }

        if (dto.ConvenioId.HasValue)
        {
            var convenioExiste = await _convenioRepository.ObterPorIdAsync(dto.ConvenioId.Value);
            if (convenioExiste == null)
            {
                resultado.AdicionarErro("ConvenioId", "Convênio não encontrado.");
            }
        }

        return resultado;
    }

    public async Task<ValidationResult> ValidarAtualizarAsync(int id, UpdatePacienteDTO dto)
    {
        var resultado = new ValidationResult();

        ValidarCamposObrigatorios(dto, resultado);
        ValidarEmail(dto.Email, resultado);
        ValidarTelefones(dto.Celular, dto.TelefoneFixo, resultado);
        ValidarDataNascimento(dto.DataNascimento, resultado);
        ValidarUfRG(dto.UfRG, resultado);

        if (!string.IsNullOrWhiteSpace(dto.CPF))
        {
            ValidarCpf(dto.CPF, resultado);
            var existeCpf = await _pacienteRepository.ExisteCpfAsync(dto.CPF, id);
            if (existeCpf)
            {
                resultado.AdicionarErro("CPF", "CPF já cadastrado no sistema.");
            }
        }

        if (dto.ConvenioId.HasValue)
        {
            var convenioExiste = await _convenioRepository.ObterPorIdAsync(dto.ConvenioId.Value);
            if (convenioExiste == null)
            {
                resultado.AdicionarErro("ConvenioId", "Convênio não encontrado.");
            }
        }

        return resultado;
    }

    private void ValidarCamposObrigatorios(CreatePacienteDTO dto, ValidationResult resultado)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
            resultado.AdicionarErro("Nome", "Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.Sobrenome))
            resultado.AdicionarErro("Sobrenome", "Sobrenome é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.RG))
            resultado.AdicionarErro("RG", "RG é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            resultado.AdicionarErro("Email", "Email é obrigatório.");
    }

    private void ValidarCamposObrigatorios(UpdatePacienteDTO dto, ValidationResult resultado)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
            resultado.AdicionarErro("Nome", "Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.Sobrenome))
            resultado.AdicionarErro("Sobrenome", "Sobrenome é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.RG))
            resultado.AdicionarErro("RG", "RG é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            resultado.AdicionarErro("Email", "Email é obrigatório.");
    }

    private void ValidarEmail(string email, ValidationResult resultado)
    {
        if (string.IsNullOrWhiteSpace(email))
            return;

        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            if (mailAddress.Address != email)
            {
                resultado.AdicionarErro("Email", "Email inválido.");
            }
        }
        catch
        {
            resultado.AdicionarErro("Email", "Email inválido.");
        }
    }

    private void ValidarTelefones(string? celular, string? telefoneFixo, ValidationResult resultado)
    {
        if (string.IsNullOrWhiteSpace(celular) && string.IsNullOrWhiteSpace(telefoneFixo))
        {
            resultado.AdicionarErro("Telefone", "Pelo menos um telefone (Celular ou Fixo) deve ser informado.");
            return;
        }

        if (!string.IsNullOrWhiteSpace(celular) && !ValidarFormatoTelefone(celular))
        {
            resultado.AdicionarErro("Celular", "Celular inválido. Use o formato (XX) XXXXX-XXXX ou (XX) XXXX-XXXX.");
        }

        if (!string.IsNullOrWhiteSpace(telefoneFixo) && !ValidarFormatoTelefone(telefoneFixo))
        {
            resultado.AdicionarErro("TelefoneFixo", "Telefone fixo inválido. Use o formato (XX) XXXX-XXXX.");
        }
    }

    private bool ValidarFormatoTelefone(string telefone)
    {
        var apenasNumeros = new string(telefone.Where(char.IsDigit).ToArray());
        return apenasNumeros.Length >= 10 && apenasNumeros.Length <= 11;
    }

    private void ValidarDataNascimento(DateTime dataNascimento, ValidationResult resultado)
    {
        if (dataNascimento > DateTime.Today)
        {
            resultado.AdicionarErro("DataNascimento", "Data de nascimento não pode ser futura.");
        }

        var idade = DateTime.Today.Year - dataNascimento.Year;
        if (dataNascimento.Date > DateTime.Today.AddYears(-idade))
            idade--;

        if (idade > 150)
        {
            resultado.AdicionarErro("DataNascimento", "Data de nascimento inválida.");
        }
    }

    private void ValidarUfRG(int ufRG, ValidationResult resultado)
    {
        if (!Enum.IsDefined(typeof(Uf), ufRG))
        {
            resultado.AdicionarErro("UfRG", "UF do RG inválida.");
        }
    }

    private void ValidarCpf(string cpf, ValidationResult resultado)
    {
        var cpfLimpo = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpfLimpo.Length != 11)
        {
            resultado.AdicionarErro("CPF", "CPF deve conter 11 dígitos.");
            return;
        }

        if (cpfLimpo.All(c => c == cpfLimpo[0]))
        {
            resultado.AdicionarErro("CPF", "CPF inválido.");
            return;
        }

        var multiplicador1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpfLimpo.Substring(0, 9);
        var soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        tempCpf += digito1;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        if (cpfLimpo[9].ToString() != digito1.ToString() || cpfLimpo[10].ToString() != digito2.ToString())
        {
            resultado.AdicionarErro("CPF", "CPF inválido.");
        }
    }
}

public class ValidationResult
{
    private readonly List<ValidationError> _erros = new();

    public bool IsValid => !_erros.Any();
    public IReadOnlyList<ValidationError> Erros => _erros.AsReadOnly();

    public void AdicionarErro(string campo, string mensagem)
    {
        _erros.Add(new ValidationError(campo, mensagem));
    }
}

public class ValidationError
{
    public string Campo { get; }
    public string Mensagem { get; }

    public ValidationError(string campo, string mensagem)
    {
        Campo = campo;
        Mensagem = mensagem;
    }
}
