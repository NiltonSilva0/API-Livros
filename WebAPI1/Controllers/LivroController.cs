using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI1.Dtos;
using WebAPI1.Interfaces;

namespace WebAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly ILivroRepositorio _livrorepositorio;
        public LivroController(ILivroRepositorio livroRepositorio)
        {
            _livrorepositorio = livroRepositorio;
        }

        [HttpPost("Criar-Livros")]
        public ActionResult CriarLivros([FromBody] List<ResponseLivroDto> livros)
        {
            if (livros == null || livros.Count == 0)
            {
                return BadRequest("A lista de livros não pode ser nula ou vazia.");
            }

            var livrosModel = new List<Models.LivroModel>();

            //Mapeamento de DTO para Model
            foreach (var livro in livros)
            {
                var livroModel = new Models.LivroModel
                {
                    Titulo = livro.Titulo,
                    Autor = livro.Autor,
                    AnoPublicacao = livro.AnoPublicacao,
                    Categoria = livro.Categoria,
                    Preco = Math.Round(livro.Preco, 2),
                    Novo = livro.Novo
                };
                livrosModel.Add(livroModel);
            }

            _livrorepositorio.Adicionar(livrosModel);

            return CreatedAtAction(nameof(ObterLivroPorId), new { id = livrosModel.First().Id }, livrosModel);
        }

        [HttpGet("Listar-Livros")]
        public ActionResult ListarLivros()
        {
            var listaLivros = _livrorepositorio.Listar();
            if (listaLivros == null || !listaLivros.Any())
            {
                return NotFound("Nenhum livro encontrado.");
            }

            return Ok(listaLivros);
        }

        [HttpGet("Obter-Livro-Por-Id/{id}")]
        public ActionResult ObterLivroPorId(string id)
        {
            var livro = _livrorepositorio.ObterPorId(id);
            if (livro == null)
            {
                return NotFound($"Livro com ID {id} não encontrado.");
            }
            return Ok(livro);
        }

        [HttpPut("Atualizar-Livro-Por-Id/{id}")]
        public ActionResult AtualizarLivroPorId(string id, [FromBody] ResponseLivroDto livro)
        {
            if (livro == null)
            {
                return BadRequest("O livro não pode ser nulo.");
            }
            var livroExistente = _livrorepositorio.ObterPorId(id);
            if (livroExistente == null)
            {
                return NotFound($"Livro com ID {id} não encontrado.");
            }
            if (livro.Preco <= 0)
            {
                return BadRequest("O preço deve ser maior que zero.");
            }
            livroExistente.Titulo = livro.Titulo;
            livroExistente.Autor = livro.Autor;
            livroExistente.AnoPublicacao = livro.AnoPublicacao;
            livroExistente.Categoria = livro.Categoria;
            livroExistente.Preco = Math.Round(livro.Preco, 2);
            livroExistente.Novo = livro.Novo;
            _livrorepositorio.Atualizar(livroExistente);
            return Ok(livroExistente);
        }

        [HttpDelete("Deletar-Livro-Por-Id/{id}")]
        public ActionResult DeletarLivroPorId(string id)
        {
            var livroExistente = _livrorepositorio.ObterPorId(id);
            if (livroExistente == null)
            {
                return NotFound($"Livro com ID {id} não encontrado.");
            }
            _livrorepositorio.Remover(id);
            return NoContent();
        }
    }
}
