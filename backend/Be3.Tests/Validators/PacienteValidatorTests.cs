using Be3.Application.DTOs;
using Be3.Application.Validators;
using Be3.Domain.Models;
using Be3.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Be3.Tests.Validators;

public class PacienteValidatorTests
{
    private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
    private readonly Mock<IConvenioRepository> _convenioRepositoryMock;
    private readonly PacienteValidator _validator;

    public PacienteValidatorTests()
    {
        _pacienteRepositoryMock = new Mock<IPacienteRepository>();
        _convenioRepositoryMock = new Mock<IConvenioRepository>();
        _validator = new PacienteValidator(
            _pacienteRepositoryMock.Object,
            _convenioRepositoryMock.Object);
    }

    [Fact]
    public async Task ValidarCriarAsync_ComDadosValidos_DeveRetornarValido()
    {
        // Arrange
        var dto = new CreatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "11999999999"
        };

        _pacienteRepositoryMock.Setup(x => x.ExisteCpfAsync(It.IsAny<string>(), null))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.ValidarCriarAsync(dto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Erros.Should().BeEmpty();
    }

    [Fact]
    public async Task ValidarCriarAsync_ComNomeVazio_DeveRetornarInvalido()
    {
        // Arrange
        var dto = new CreatePacienteDTO
        {
            Nome = "",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "11999999999"
        };

        // Act
        var result = await _validator.ValidarCriarAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Erros.Should().Contain(e => e.Campo == "Nome" && e.Mensagem == "Nome é obrigatório.");
    }

    [Fact]
    public async Task ValidarCriarAsync_ComSobrenomeVazio_DeveRetornarInvalido()
    {
        // Arrange
        var dto = new CreatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "11999999999"
        };

        // Act
        var result = await _validator.ValidarCriarAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Erros.Should().Contain(e => e.Campo == "Sobrenome" && e.Mensagem == "Sobrenome é obrigatório.");
    }

    [Fact]
    public async Task ValidarCriarAsync_ComEmailInvalido_DeveRetornarInvalido()
    {
        // Arrange
        var dto = new CreatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "email-invalido",
            Celular = "11999999999"
        };

        // Act
        var result = await _validator.ValidarCriarAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Erros.Should().Contain(e => e.Campo == "Email" && e.Mensagem == "Email inválido.");
    }

    [Fact]
    public async Task ValidarCriarAsync_SemTelefones_DeveRetornarInvalido()
    {
        // Arrange
        var dto = new CreatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "",
            TelefoneFixo = ""
        };

        // Act
        var result = await _validator.ValidarCriarAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Erros.Should().Contain(e => e.Campo == "Telefone" && e.Mensagem == "Pelo menos um telefone (Celular ou Fixo) deve ser informado.");
    }

    [Fact]
    public async Task ValidarCriarAsync_ComDataNascimentoFutura_DeveRetornarInvalido()
    {
        // Arrange
        var dto = new CreatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = DateTime.Today.AddDays(1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "11999999999"
        };

        // Act
        var result = await _validator.ValidarCriarAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Erros.Should().Contain(e => e.Campo == "DataNascimento");
    }

    [Fact]
    public async Task ValidarCriarAsync_ComUfRGInvalida_DeveRetornarInvalido()
    {
        // Arrange
        var dto = new CreatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 99, // UF inválida
            Email = "joao@email.com",
            Celular = "11999999999"
        };

        // Act
        var result = await _validator.ValidarCriarAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Erros.Should().Contain(e => e.Campo == "UfRG" && e.Mensagem == "UF do RG inválida.");
    }

    [Fact]
    public async Task ValidarCriarAsync_ComCpfInvalido_DeveRetornarInvalido()
    {
        // Arrange
        var dto = new CreatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            CPF = "12345678901", // CPF inválido
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "11999999999"
        };

        // Act
        var result = await _validator.ValidarCriarAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Erros.Should().Contain(e => e.Campo == "CPF");
    }

    [Fact]
    public async Task ValidarCriarAsync_ComCpfJaCadastrado_DeveRetornarInvalido()
    {
        // Arrange
        var dto = new CreatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            CPF = "11144477735", // CPF válido
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "11999999999"
        };

        _pacienteRepositoryMock.Setup(x => x.ExisteCpfAsync("11144477735", null))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.ValidarCriarAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Erros.Should().Contain(e => e.Campo == "CPF" && e.Mensagem == "CPF já cadastrado no sistema.");
    }

    [Fact]
    public async Task ValidarCriarAsync_ComConvenioInexistente_DeveRetornarInvalido()
    {
        // Arrange
        var dto = new CreatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "11999999999",
            ConvenioId = 999
        };

        _convenioRepositoryMock.Setup(x => x.ObterPorIdAsync(999))
            .ReturnsAsync((Convenio?)null);

        // Act
        var result = await _validator.ValidarCriarAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Erros.Should().Contain(e => e.Campo == "ConvenioId" && e.Mensagem == "Convênio não encontrado.");
    }

    [Fact]
    public async Task ValidarAtualizarAsync_ComDadosValidos_DeveRetornarValido()
    {
        // Arrange
        var dto = new UpdatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "11999999999"
        };

        _pacienteRepositoryMock.Setup(x => x.ExisteCpfAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.ValidarAtualizarAsync(1, dto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Erros.Should().BeEmpty();
    }

    [Fact]
    public async Task ValidarAtualizarAsync_ComCpfJaCadastradoParaOutroPaciente_DeveRetornarInvalido()
    {
        // Arrange
        var dto = new UpdatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            CPF = "11144477735",
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "11999999999"
        };

        _pacienteRepositoryMock.Setup(x => x.ExisteCpfAsync("11144477735", 1))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.ValidarAtualizarAsync(1, dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Erros.Should().Contain(e => e.Campo == "CPF" && e.Mensagem == "CPF já cadastrado no sistema.");
    }
}

