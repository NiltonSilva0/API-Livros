using Microsoft.AspNetCore.Mvc;
using WebAPI1.Dtos;
using WebAPI1.Interfaces;
using WebAPI1.Models;

namespace WebAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroServicosController : ControllerBase
    {
        private readonly ILivro _livroService;
        private readonly ILivroRepositorio _livroRepositorio;

        public LivroServicosController(ILivro livroService, ILivroRepositorio livroRepositorio)
        {
            _livroService = livroService;
            _livroRepositorio = livroRepositorio;
        }

        [HttpPost("Emprestar-Livro-Por-Id/{id}")]
        public ActionResult EmprestarLivroPorId(string id)
        {
            var livro = _livroRepositorio.ObterPorId(id);
            if (livro == null)
            {
                return NotFound($"Livro com ID {id} não encontrado.");
            }
            if (!_livroService.EmprestarLivroPorId(id))
            {
                return BadRequest("O livro não está disponível para empréstimo.");
            }
            return Ok(livro);
        }

        [HttpPost("Devolver-Livro-Por-Id/{id}")]
        public ActionResult DevolverLivroPorId(string id)
        {
            var livro = _livroRepositorio.ObterPorId(id);
            if (livro == null)
            {
                return NotFound($"Livro com ID {id} não encontrado.");
            }
            if (!_livroService.DevolverLivroPorId(id))
            {
                return BadRequest("O livro já está disponível na biblioteca.");
            }
            return Ok(livro);
        }

        [HttpPost("Vender-Livro/{id}")]
        public ActionResult VenderLivro(string id)
        {
            var livro = _livroRepositorio.ObterPorId(id);
            if (livro == null)
            {
                return NotFound($"Livro com ID {id} não encontrado.");
            }
            if (livro.Vendido)
            {
                return BadRequest("O livro já foi vendido.");
            }
            if (!_livroService.VenderLivro(id))
            {
                return BadRequest("Não foi possível vender o livro.");
            }
            return Ok(livro);
        }

        [HttpPost("Promocao-Livro/{id}")]
        public ActionResult PromocaoLivro(string id)
        {
            var livro = _livroRepositorio.ObterPorId(id);
            if (livro == null)
            {
                return NotFound($"Livro com ID {id} não encontrado.");
            }
            if (!_livroService.EmPromocao(id))
            {
                return BadRequest("Não foi possível colocar o livro em promoção.");
            }
            return Ok(livro);
        }

        [HttpGet("Historico-Vendidos")]
        public ActionResult<List<LivroModel>> ListarHistoricoVendidos()
        {
            var historico = _livroService.ListarHistoricoVendidos();
            if (historico == null || !historico.Any())
                return NotFound("Nenhum livro vendido encontrado.");
            return Ok(historico);
        }

        [HttpGet("Livros-Em-Promocao")]
        public ActionResult<List<LivroModel>> ListarLivrosEmPromocao()
        {
            var promocao = _livroService.ListarLivrosEmPromocao();
            if (promocao == null || !promocao.Any())
                return NotFound("Nenhum livro em promoção encontrado.");
            return Ok(promocao);
        }

        [HttpGet("Livros-Novos")]
        public ActionResult<List<LivroModel>> ListarLivrosNovos()
        {
            var novos = _livroService.ListarLivrosNovos();
            if (novos == null || !novos.Any())
                return NotFound("Nenhum livro novo encontrado.");
            return Ok(novos);
        }

        [HttpGet("Livros-Usados")]
        public ActionResult<List<LivroModel>> ListarLivrosUsados()
        {
            var usados = _livroService.ListarLivrosUsados();
            if (usados == null || !usados.Any())
                return NotFound("Nenhum livro usado encontrado.");
            return Ok(usados);
        }
    }
}
