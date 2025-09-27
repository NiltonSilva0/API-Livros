using System;
using System.Collections.Generic;
using Xunit;
using WebAPI1.Models.Cliente;
using WebAPI1.Dtos.Cliente;

namespace WebAPI1.Tests
{
    public class ClienteRepositorioFake
    {
        public List<ClienteModel> Clientes { get; set; } = new();

        public List<ClienteModel> AdicionarAsync(List<CriarOuAtualizaClienteDto> clientesDto)
        {
            var now = DateTime.UtcNow;
            var clientes = clientesDto.Select(dto => new ClienteModel
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Genero = dto.Genero,
                DataNascimento = dto.DataNascimento,
                DataCadastro = now,
                Ativo = true
            }).ToList();

            Clientes.AddRange(clientes);
            return clientes;
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void AdicionarAsync_DeveDefinirDataCadastroCorretamente()
        {
            // Arrange
            var repo = new ClienteRepositorioFake();
            var dto = new CriarOuAtualizaClienteDto
            {
                Nome = "Teste",
                Email = "teste@email.com",
                Telefone = "123456789",
                Genero = "Outro",
                DataNascimento = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            var dtos = new List<CriarOuAtualizaClienteDto> { dto };

            // Act
            var before = DateTime.UtcNow;
            var result = repo.AdicionarAsync(dtos);
            var after = DateTime.UtcNow;

            // Assert
            Assert.Single(result);
            Assert.Equal("Teste", result[0].Nome);
            Assert.Equal("teste@email.com", result[0].Email);
            Assert.Equal("123456789", result[0].Telefone);
            Assert.Equal("Outro", result[0].Genero);
            Assert.Equal(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), result[0].DataNascimento);
            Assert.True(result[0].Ativo);
            Assert.True(result[0].DataCadastro > DateTime.MinValue);
            Assert.InRange(result[0].DataCadastro, before, after);
        }

        [Fact]
        public void AdicionarAsync_DeveAdicionarVariosClientes()
        {
            // Arrange
            var repo = new ClienteRepositorioFake();
            var dtos = new List<CriarOuAtualizaClienteDto>
            {
                new CriarOuAtualizaClienteDto { Nome = "A", Email = "a@email.com", Telefone = "1", Genero = "M", DataNascimento = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc) },
                new CriarOuAtualizaClienteDto { Nome = "B", Email = "b@email.com", Telefone = "2", Genero = "F", DataNascimento = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc) }
            };

            // Act
            var result = repo.AdicionarAsync(dtos);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.True(c.DataCadastro > DateTime.MinValue));
        }

        [Fact]
        public void AdicionarAsync_DeveRetornarListaVazia_SeNenhumDto()
        {
            // Arrange
            var repo = new ClienteRepositorioFake();
            var dtos = new List<CriarOuAtualizaClienteDto>();

            // Act
            var result = repo.AdicionarAsync(dtos);

            // Assert
            Assert.Empty(result);
        }
    }
}
