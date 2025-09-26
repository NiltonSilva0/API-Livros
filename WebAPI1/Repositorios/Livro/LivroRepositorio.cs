using WebAPI1.Interfaces.Livro;
using WebAPI1.Models.Livro;

namespace WebAPI1.Repositorios.Livro
{
    public class LivroRepositorio : ILivroRepositorio
    {
        private static List<LivroModel> livros = new();

        public void Adicionar(List<LivroModel> livro)
        {
            if (livro == null)
                throw new ArgumentNullException(nameof(livro));

            foreach (var item in livro)
            {
                if (item.Preco <= 0)
                {
                    throw new ArgumentException($"O preço do livro '{item.Titulo}' deve ser maior que zero.");
                }
            }

            livros.AddRange(livro);
        }

        public bool Atualizar(LivroModel livro)
        {
            var livroExistente = livros.FirstOrDefault(l => l.Id == livro.Id);
            if (livroExistente != null)
            {
                livroExistente.Titulo = livro.Titulo;
                livroExistente.Autor = livro.Autor;
                livroExistente.AnoPublicacao = livro.AnoPublicacao;
                livroExistente.Categoria = livro.Categoria;
                livroExistente.Preco = livro.Preco;
                livroExistente.Novo = livro.Novo;
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<LivroModel> Listar()
        {
            return livros.Where(l => !l.Inativo && !l.Vendido).ToList();
        }

        public LivroModel? ObterPorId(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var livro = livros.FirstOrDefault(l => l.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
                return livro;
            }
            return null;
        }

        public bool Remover(string id)
        {
            var livro = livros.FirstOrDefault(l => l.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (livro != null)
            {
                livro.Inativo = true;
                return true;
            }
            return false;
        }

        List<LivroModel> ILivroRepositorio.ListarInativos(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return livros.Where(l => l.Inativo && l.Id.Equals(id, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                return livros.Where(l => l.Inativo).ToList();
            }
        }

        List<LivroModel> ILivroRepositorio.ListarPorAno(int ano)
        {
            if (ano > 1700)
            {
                return livros.Where(l => l.AnoPublicacao == ano && !l.Inativo).ToList();
            }
            else
            {
                return new List<LivroModel>();
            }
        }

        List<LivroModel> ILivroRepositorio.ListarPorAutor(string autor)
        {
            if (!String.IsNullOrEmpty(autor))
            {
                return livros.Where(l => l.Autor.Contains(autor, StringComparison.OrdinalIgnoreCase) && !l.Inativo).ToList();
            }
            else
            {
                return new List<LivroModel>();
            }
        }

        List<LivroModel> ILivroRepositorio.ListarPorCategoria(string categoria)
        {
            if (!String.IsNullOrEmpty(categoria))
            {
                return livros.Where(l => l.Categoria.Contains(categoria, StringComparison.OrdinalIgnoreCase) && !l.Inativo).ToList();
            }
            else
            {
                return new List<LivroModel>();
            }
        }

        List<LivroModel> ILivroRepositorio.ListarPorTitulo(string titulo)
        {
            if (!String.IsNullOrEmpty(titulo))
            {
                return livros.Where(l => l.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase) && !l.Inativo).ToList();
            }
            else
            {
                return new List<LivroModel>();
            }
        }
    }
}