using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI1.Dtos.Cliente;
using WebAPI1.Interfaces.Cliente;
using WebAPI1.Models.Cliente;

namespace WebAPI1.Controllers.Cliente
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController(IClienteRepositorio clienteRepositorio) : ControllerBase
    {
        [HttpGet("Listar-Clientes")]
        public ActionResult ListarTodos()
        {
            try
            {
                var clientes = clienteRepositorio.Listar();
                if (clientes == null)
                {
                    return NotFound("Nenhum cliente encontrado.");
                }
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                var correlationId = HttpContext.TraceIdentifier;
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "Erro interno no servidor.",
                    CorrelationId = correlationId,
                    ExceptionMessage = ex.Message
                });
            }
        }
        [HttpGet("Obter-Cliente-Por-Id/{id}")]
        public ActionResult ListarPorId(string id)
        {
            try
            {
                var cliente = clienteRepositorio.ObterPorId(id);
                if (cliente == null)
                {
                    return NotFound($"Cliente com ID {id} não encontrado.");
                }
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                var correlationId = HttpContext.TraceIdentifier;
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "Erro interno no servidor.",
                    CorrelationId = correlationId,
                    ExceptionMessage = ex.Message
                });
            }
        }
        [HttpGet("Listar-Clientes-Inativos")]
        public ActionResult ListarInativos(string name)
        {
            try
            {
                var clientes = clienteRepositorio.ListarInativos();
                if (clientes == null || !clientes.Any())
                {
                    return NotFound("Nenhum cliente inativo encontrado.");
                }
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                var correlationId = HttpContext.TraceIdentifier;
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "Erro interno no servidor.",
                    CorrelationId = correlationId,
                    ExceptionMessage = ex.Message
                });
            }
        }
        [HttpGet("Listar-Clientes-Por-Genero")]
        public ActionResult ListarPorGenero()
        {
            try
            {
                var clientesPorGenero = clienteRepositorio.ListarPorGenero();
                if (clientesPorGenero == null || !clientesPorGenero.Any())
                {
                    return NotFound("Nenhum cliente encontrado por gênero.");
                }
                return Ok(clientesPorGenero);
            }
            catch (Exception ex)
            {
                var correlationId = HttpContext.TraceIdentifier;
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "Erro interno no servidor.",
                    CorrelationId = correlationId,
                    ExceptionMessage = ex.Message
                });
            }
        }
        [HttpPost("Adicionar-Cliente")]
        public async Task<ActionResult> Adicionar([FromBody] List<CriarOuAtualizaClienteDto> clientes)
        {
            try
            {
                if (clientes == null || !clientes.Any())
                {
                    return BadRequest("A lista de clientes não pode ser nula ou vazia.");
                }

                var novosClientes = clientes.Select(item => new ClienteModel
                {
                    Nome = item.Nome,
                    Email = item.Email,
                    Telefone = item.Telefone,
                    Genero = item.Genero,
                    DataNascimento = item.DataNascimento,
                    DataCadastro = DateTime.Now,
                    Ativo = true
                }).ToList();

                var clientesAdicionados = await clienteRepositorio.AdicionarAsync(clientes);

                return Ok(clientesAdicionados);
            }
            catch (Exception ex)
            {
                var correlationId = HttpContext.TraceIdentifier;
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "Erro interno no servidor.",
                    CorrelationId = correlationId,
                    ExceptionMessage = ex.Message
                });
            }
        }
        [HttpPut("Atualizar-Cliente")]
        public ActionResult Atualizar([FromBody] CriarOuAtualizaClienteDto cliente, [FromQuery] string id)
        {
            try
            {
                if (cliente == null)
                {
                    return BadRequest("O cliente não pode ser nulo.");
                }
                var clienteExistente = clienteRepositorio.ObterPorId(id);
                if (clienteExistente == null)
                {
                    return NotFound($"Cliente com ID {id} não encontrado.");
                }
                clienteExistente.Nome = cliente.Nome;
                clienteExistente.Email = cliente.Email;
                clienteExistente.Telefone = cliente.Telefone;
                clienteExistente.Genero = cliente.Genero;
                clienteExistente.DataNascimento = cliente.DataNascimento;
                var atualizado = clienteRepositorio.Atualizar(clienteExistente);
                return Ok(atualizado);
            }
            catch (Exception ex)
            {
                var correlationId = HttpContext.TraceIdentifier;
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "Erro interno no servidor.",
                    CorrelationId = correlationId,
                    ExceptionMessage = ex.Message
                });
            }
        }
        [HttpDelete("Remover-Cliente/{id}")]
        public ActionResult Remover(string id)
        {
            try
            {
                var clienteExistente = clienteRepositorio.ObterPorId(id);
                if (clienteExistente == null)
                {
                    return NotFound($"Cliente com ID {id} não encontrado.");
                }
                var removido = clienteRepositorio.Remover(id);
                return Ok($"Cliente com ID {id} removido com sucesso.");
            }
            catch (Exception ex)
            {
                var correlationId = HttpContext.TraceIdentifier;
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "Erro interno no servidor.",
                    CorrelationId = correlationId,
                    ExceptionMessage = ex.Message
                });
            }
        }
    }
}
