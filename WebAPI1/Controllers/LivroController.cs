using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI1.Dtos;
using WebAPI1.Interfaces;

namespace WebAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController(ILivroRepositorio _livroRepositorio) : ControllerBase
    {
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

            _livroRepositorio.Adicionar(livrosModel);

            return CreatedAtAction(nameof(ObterLivroPorId), new { id = livrosModel.First().Id }, livrosModel);
        }

        [HttpGet("Listar-Livros")]
        public ActionResult ListarLivros()
        {
            var listaLivros = _livroRepositorio.Listar();
            if (listaLivros == null || !listaLivros.Any())
            {
                return NotFound("Nenhum livro encontrado.");
            }

            return Ok(listaLivros);
        }

        [HttpGet("Obter-Livro-Por-Id/{id}")]
        public ActionResult ObterLivroPorId(string id)
        {
            var livro = _livroRepositorio.ObterPorId(id);
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
            var livroExistente = _livroRepositorio.ObterPorId(id);
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
            _livroRepositorio.Atualizar(livroExistente);
            return Ok(livroExistente);
        }

        [HttpDelete("Deletar-Livro-Por-Id/{id}")]
        public ActionResult DeletarLivroPorId(string id)
        {
            var livroExistente = _livroRepositorio.ObterPorId(id);
            if (livroExistente == null)
            {
                return NotFound($"Livro com ID {id} não encontrado.");
            }
            _livroRepositorio.Remover(id);
            return NoContent();
        }
    }
}
