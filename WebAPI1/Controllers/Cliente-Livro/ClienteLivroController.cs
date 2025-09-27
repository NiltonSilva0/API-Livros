using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI1.Repositorios.Cliente_Livro;
using WebAPI1.Models.Cliente_Livro;

namespace WebAPI1.Controllers.Cliente_Livro
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteLivroController(IClienteLivroRepositorio clienteLivroService) : ControllerBase
    {
        [HttpPost("emprestar")]
        public IActionResult Emprestar([FromQuery] string clienteId, [FromQuery] string livroId)
        {
            try
            {
                clienteLivroService.RegistrarEmprestimo(clienteId, livroId);
                return Ok("Empréstimo registrado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("devolver")]
        public IActionResult Devolver([FromQuery] string clienteId, [FromQuery] string livroId)
        {
            try
            {
                clienteLivroService.RegistrarDevolucao(clienteId, livroId);
                return Ok("Devolução registrada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("vender")]
        public IActionResult Vender([FromQuery] string clienteId, [FromQuery] string livroId)
        {
            try
            {
                clienteLivroService.RegistrarVenda(clienteId, livroId);
                return Ok("Venda registrada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("listar-por-cliente/{clienteId}")]
        public IActionResult ListarPorCliente(string clienteId)
        {
            try
            {
                var registros = clienteLivroService.ListarPorCliente(clienteId);
                return Ok(registros);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("listar-por-livro/{livroId}")]
        public IActionResult ListarPorLivro(string livroId)
        {
            try
            {
                var registros = clienteLivroService.ListarPorLivro(livroId);
                return Ok(registros);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
