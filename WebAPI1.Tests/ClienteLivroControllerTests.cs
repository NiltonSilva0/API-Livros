using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI1.Controllers.Cliente_Livro;
using WebAPI1.Models.Cliente_Livro;
using WebAPI1.Models.Cliente;
using WebAPI1.Models.Livro;
using WebAPI1.Repositorios.Cliente;
using WebAPI1.Repositorios.Livro;
using WebAPI1.Services.Cliente_Livro;
using WebAPI1.Dtos.Cliente;

namespace WebAPI1.Tests
{
    public class ClienteLivroControllerTests
    {
        private ClienteRepositorio clienteRepo;
        private LivroRepositorio livroRepo;
        private ClienteLivroService service;
        private ClienteLivroController controller;

        public ClienteLivroControllerTests()
        {
            clienteRepo = new ClienteRepositorio();
            livroRepo = new LivroRepositorio();
            service = new ClienteLivroService(clienteRepo, livroRepo);
            controller = new ClienteLivroController(service);

            // Adiciona cliente com todos os campos obrigatórios preenchidos
            var cliente = new ClienteModel
            {
                Id = "c1",
                Nome = "Cliente Teste",
                Email = "cliente@teste.com",
                Telefone = "11999999999",
                Genero = "M",
                DataNascimento = new DateTime(1990, 1, 1),
                Ativo = true
            };
            clienteRepo.AdicionarAsync(new List<CriarOuAtualizaClienteDto> {
                new CriarOuAtualizaClienteDto {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Telefone = cliente.Telefone,
                    Genero = cliente.Genero,
                    DataNascimento = cliente.DataNascimento,
                    Ativo = cliente.Ativo
                }
            }).Wait();

            var livro = new LivroModel {
                Id = "l1",
                Titulo = "Livro Teste",
                Autor = "Autor Teste",
                AnoPublicacao = 2020,
                Categoria = "Ficção",
                Preco = 10.0m,
                Disponivel = true,
                Novo = true
            };
            livroRepo.Adicionar(new List<LivroModel> { livro });
        }

        [Fact]
        public void Emprestar_DeveRegistrarEmprestimo()
        {
            var result = controller.Emprestar("c1", "l1");
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Emprestar_DeveFalhar_SeClienteNaoExiste()
        {
            var result = controller.Emprestar("c2", "l1");
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Cliente com ID c2 não encontrado", badRequest.Value.ToString());
        }

        [Fact]
        public void Emprestar_DeveFalhar_SeLivroNaoExiste()
        {
            var result = controller.Emprestar("c1", "l2");
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Livro com ID l2 não encontrado", badRequest.Value.ToString());
        }

        [Fact]
        public void Emprestar_DeveFalhar_SeLivroIndisponivel()
        {
            controller.Emprestar("c1", "l1");
            var result = controller.Emprestar("c1", "l1");
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("já possui um empréstimo ativo", badRequest.Value.ToString());
        }

        [Fact]
        public void Devolver_DeveRegistrarDevolucao()
        {
            controller.Emprestar("c1", "l1");
            var result = controller.Devolver("c1", "l1");
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Devolver_DeveFalhar_SeNaoEmprestado()
        {
            var result = controller.Devolver("c1", "l1");
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Nenhum empréstimo ativo encontrado", badRequest.Value.ToString());
        }

        [Fact]
        public void Vender_DeveRegistrarVenda()
        {
            // Adiciona novo livro disponível
            var livro = new LivroModel { Id = "l2", Titulo = "Livro Venda", Disponivel = true, Novo = true };
            livroRepo.Adicionar(new List<LivroModel> { livro });
            var result = controller.Vender("c1", "l2");
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Vender_DeveFalhar_SeClienteNaoExiste()
        {
            var livro = new LivroModel { Id = "l3", Titulo = "Livro Venda", Autor = "Autor Teste", Disponivel = true, Novo = true };
            livroRepo.Adicionar(new List<LivroModel> { livro });
            var result = controller.Vender("c2", "l3");
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Cliente com ID c2 não encontrado", badRequest.Value.ToString());
        }

        [Fact]
        public void Vender_DeveFalhar_SeLivroNaoExiste()
        {
            // Garante que o cliente existe
            var cliente = new ClienteModel
            {
                Id = "c3",
                Nome = "Cliente Teste 3",
                Email = "cliente3@teste.com",
                Telefone = "11977776666",
                Genero = "M",
                DataNascimento = new DateTime(1993, 3, 3),
                Ativo = true
            };
            clienteRepo.AdicionarAsync(new List<CriarOuAtualizaClienteDto> {
                new CriarOuAtualizaClienteDto {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Telefone = cliente.Telefone,
                    Genero = cliente.Genero,
                    DataNascimento = cliente.DataNascimento,
                    Ativo = cliente.Ativo
                }
            }).Wait();
            var result = controller.Vender("c3", "l4");
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Livro com ID l4 não encontrado", badRequest.Value.ToString());
        }

        [Fact]
        public void Vender_DeveFalhar_SeJaComprou()
        {
            var livro = new LivroModel { Id = "l6", Titulo = "Livro Já Comprado", Autor = "Autor Teste", Disponivel = true, Novo = true };
            livroRepo.Adicionar(new List<LivroModel> { livro });
            controller.Vender("c1", "l6");
            var result = controller.Vender("c1", "l6");
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("já comprou o livro", badRequest.Value.ToString());
        }

        [Fact]
        public void Vender_DeveFalhar_SeLivroIndisponivel()
        {
            var livro = new LivroModel { Id = "l5", Titulo = "Livro Indisponível", Autor = "Autor Teste", Disponivel = false, Novo = true };
            livroRepo.Adicionar(new List<LivroModel> { livro });
            var result = controller.Vender("c1", "l5");
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Livro com ID l5 não encontrado ou indisponível", badRequest.Value.ToString());
        }

        [Fact]
        public async Task ListarPorCliente_DeveRetornarRegistros()
        {
            var cliente = new ClienteModel
            {
                Id = "c2",
                Nome = "Cliente Teste 2",
                Email = "cliente2@teste.com",
                Telefone = "11988887777",
                Genero = "F",
                DataNascimento = new DateTime(1992, 2, 2),
                Ativo = true
            };

            var criarDto = new CriarOuAtualizaClienteDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Telefone = cliente.Telefone,
                Genero = cliente.Genero,
                DataNascimento = cliente.DataNascimento,
                Ativo = cliente.Ativo
            };

            await clienteRepo.AdicionarAsync(new List<CriarOuAtualizaClienteDto> { criarDto });

            var livro = new LivroModel
            {
                Id = "l7",
                Titulo = "Livro Cliente",
                Autor = "Autor Teste",
                AnoPublicacao = 2020,
                Categoria = "Ficção",
                Preco = 10.0m,
                Disponivel = true,
                Novo = true
            };
            livroRepo.Adicionar(new List<LivroModel> { livro });
            controller.Emprestar("c2", "l7");
            var result = controller.ListarPorCliente("c2");
            if (result is OkObjectResult okResult)
            {
                var registros = Assert.IsAssignableFrom<List<ClienteLivroModel>>(okResult.Value);
                Assert.NotEmpty(registros);
            }
            else if (result is NotFoundObjectResult notFoundResult)
            {
                Assert.False(true, $"ListarPorCliente retornou NotFound: {notFoundResult.Value}");
            }
            else
            {
                Assert.False(true, $"ListarPorCliente retornou tipo inesperado: {result.GetType().Name}");
            }
        }

        [Fact]
        public void ListarPorLivro_DeveRetornarRegistros()
        {
            var livro = new LivroModel { Id = "l8", Titulo = "Livro Listar", Autor = "Autor Teste", Disponivel = true, Novo = true };
            livroRepo.Adicionar(new List<LivroModel> { livro });
            controller.Emprestar("c1", "l8");
            var result = controller.ListarPorLivro("l8");
            var okResult = Assert.IsType<OkObjectResult>(result);
            var registros = Assert.IsAssignableFrom<List<ClienteLivroModel>>(okResult.Value);
            Assert.NotEmpty(registros);
        }
    }
}