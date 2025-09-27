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
                if (item == null) continue;

                // Validações mínimas para suportar os cenários de teste
                if (string.IsNullOrWhiteSpace(item.Titulo))
                {
                    throw new ArgumentException("O título do livro não pode ser vazio.");
                }

                if (string.IsNullOrWhiteSpace(item.Id))
                {
                    item.Id = Guid.NewGuid().ToString().Substring(0, 8);
                }

                var existente = livros.FirstOrDefault(l => l.Id.Equals(item.Id, StringComparison.OrdinalIgnoreCase));
                if (existente == null)
                {
                    livros.Add(item);
                }
                else
                {
                    // Atualiza campos básicos se já existir
                    existente.Titulo = string.IsNullOrWhiteSpace(item.Titulo) ? existente.Titulo : item.Titulo;
                    if (!string.IsNullOrWhiteSpace(item.Autor)) existente.Autor = item.Autor;
                    if (item.AnoPublicacao > 0) existente.AnoPublicacao = item.AnoPublicacao;
                    if (!string.IsNullOrWhiteSpace(item.Categoria)) existente.Categoria = item.Categoria;
                    if (item.Preco > 0) existente.Preco = item.Preco;
                    existente.Disponivel = item.Disponivel;
                    existente.Novo = item.Novo;
                    existente.Inativo = item.Inativo;
                    existente.Vendido = item.Vendido;
                }
            }
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
                livroExistente.Disponivel = livro.Disponivel;
                livroExistente.Inativo = livro.Inativo;
                livroExistente.Vendido = livro.Vendido;
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