using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI1.Controllers.Cliente;
using WebAPI1.Dtos.Cliente;
using WebAPI1.Interfaces.Cliente;
using WebAPI1.Models.Cliente;
using Xunit;

namespace WebAPI1.Tests
{
    public class ClienteControllerTests
    {
        private ClienteController GetController(Mock<IClienteRepositorio> mockRepo)
        {
            return new ClienteController(mockRepo.Object)
            {
                ControllerContext = new ControllerContext()
            };
        }

        [Fact]
        public void ListarTodos_DeveRetornarOkComClientes()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            mockRepo.Setup(r => r.Listar()).Returns(new List<ClienteModel> { new ClienteModel { Nome = "Teste" } });

            var controller = GetController(mockRepo);

            var result = controller.ListarTodos();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var clientes = Assert.IsAssignableFrom<List<ClienteModel>>(okResult.Value);
            Assert.Single(clientes);
            Assert.Equal("Teste", clientes[0].Nome);
        }

        [Fact]
        public void ListarTodos_DeveRetornarNotFound()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            mockRepo.Setup(r => r.Listar()).Returns((List<ClienteModel>)null);

            var controller = GetController(mockRepo);

            var result = controller.ListarTodos();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ListarPorId_DeveRetornarOk()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            mockRepo.Setup(r => r.ObterPorId("1")).Returns(new ClienteModel { Id = "1", Nome = "Teste" });

            var controller = GetController(mockRepo);

            var result = controller.ListarPorId("1");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var cliente = Assert.IsType<ClienteModel>(okResult.Value);
            Assert.Equal("1", cliente.Id);
        }

        [Fact]
        public void ListarPorId_DeveRetornarNotFound()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            mockRepo.Setup(r => r.ObterPorId("1")).Returns((ClienteModel)null);

            var controller = GetController(mockRepo);

            var result = controller.ListarPorId("1");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ListarInativos_DeveRetornarOk()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            mockRepo.Setup(r => r.ListarInativos()).Returns(new List<ClienteModel> { new ClienteModel { Ativo = false } });

            var controller = GetController(mockRepo);

            var result = controller.ListarInativos("qualquer");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var clientes = Assert.IsAssignableFrom<List<ClienteModel>>(okResult.Value);
            Assert.Single(clientes);
            Assert.False(clientes[0].Ativo);
        }

        [Fact]
        public void ListarInativos_DeveRetornarNotFound()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            mockRepo.Setup(r => r.ListarInativos()).Returns(new List<ClienteModel>());

            var controller = GetController(mockRepo);

            var result = controller.ListarInativos("qualquer");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ListarPorGenero_DeveRetornarOk()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            mockRepo.Setup(r => r.ListarPorGenero()).Returns(new Dictionary<string, List<ClienteModel>>
            {
                ["M"] = new List<ClienteModel> { new ClienteModel { Genero = "M" } }
            });

            var controller = GetController(mockRepo);

            var result = controller.ListarPorGenero();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var clientesPorGenero = Assert.IsAssignableFrom<Dictionary<string, List<ClienteModel>>>(okResult.Value);
            Assert.True(clientesPorGenero.ContainsKey("M"));
            var clientes = clientesPorGenero["M"];
            Assert.Single(clientes);
            Assert.Equal("M", clientes[0].Genero);
        }

        [Fact]
        public void ListarPorGenero_DeveRetornarNotFound()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            mockRepo.Setup(r => r.ListarPorGenero()).Returns(new Dictionary<string, List<ClienteModel>>());

            var controller = GetController(mockRepo);

            var result = controller.ListarPorGenero();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Adicionar_DeveRetornarBadRequest_SeListaVazia()
        {
            var mockRepo = new Mock<IClienteRepositorio>();

            var controller = GetController(mockRepo);

            var result = await controller.Adicionar(new List<CriarOuAtualizaClienteDto>());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Adicionar_DeveRetornarOk()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            var dtos = new List<CriarOuAtualizaClienteDto>
            {
                new CriarOuAtualizaClienteDto { Nome = "Novo", Email = "novo@email.com", Telefone = "123", Genero = "M", DataNascimento = DateTime.Today }
            };
            var models = new List<ClienteModel>
            {
                new ClienteModel { Nome = "Novo", Email = "novo@email.com", Telefone = "123", Genero = "M", DataNascimento = DateTime.Today, DataCadastro = DateTime.Now, Ativo = true }
            };
            mockRepo.Setup(r => r.AdicionarAsync(dtos)).ReturnsAsync(models);

            var controller = GetController(mockRepo);

            var result = await controller.Adicionar(dtos);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var clientes = Assert.IsAssignableFrom<List<ClienteModel>>(okResult.Value);
            Assert.Single(clientes);
            Assert.Equal("Novo", clientes[0].Nome);
        }

        [Fact]
        public void Atualizar_DeveRetornarBadRequest_SeClienteNulo()
        {
            var mockRepo = new Mock<IClienteRepositorio>();

            var controller = GetController(mockRepo);

            var result = controller.Atualizar(null, "1");

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Atualizar_DeveRetornarNotFound_SeClienteNaoExiste()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            mockRepo.Setup(r => r.ObterPorId("1")).Returns((ClienteModel)null);

            var controller = GetController(mockRepo);

            var dto = new CriarOuAtualizaClienteDto { Nome = "Teste" };
            var result = controller.Atualizar(dto, "1");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Atualizar_DeveRetornarOk()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            var clienteExistente = new ClienteModel { Id = "1", Nome = "Antigo" };
            mockRepo.Setup(r => r.ObterPorId("1")).Returns(clienteExistente);
            mockRepo.Setup(r => r.Atualizar(It.IsAny<ClienteModel>())).Returns(clienteExistente);

            var controller = GetController(mockRepo);

            var dto = new CriarOuAtualizaClienteDto
            {
                Nome = "Novo",
                Email = "novo@email.com",
                Telefone = "123",
                Genero = "M",
                DataNascimento = DateTime.Today
            };

            var result = controller.Atualizar(dto, "1");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var cliente = Assert.IsType<ClienteModel>(okResult.Value);
            Assert.Equal("Novo", cliente.Nome);
        }

        [Fact]
        public void Remover_DeveRetornarNotFound_SeClienteNaoExiste()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            mockRepo.Setup(r => r.ObterPorId("1")).Returns((ClienteModel)null);

            var controller = GetController(mockRepo);

            var result = controller.Remover("1");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Remover_DeveRetornarOk()
        {
            var mockRepo = new Mock<IClienteRepositorio>();
            mockRepo.Setup(r => r.ObterPorId("1")).Returns(new ClienteModel { Id = "1" });
            mockRepo.Setup(r => r.Remover("1")).Returns(new ClienteModel { Id = "1" });

            var controller = GetController(mockRepo);

            var result = controller.Remover("1");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("removido com sucesso", okResult.Value.ToString());
        }
    }
}