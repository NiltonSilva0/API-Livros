using WebAPI1.Interfaces.Livro;
using WebAPI1.Models.Livro;
using WebAPI1.Repositorios;

namespace WebAPI1.Services.Livro
{
    public class LivroService : ILivro
    {
        private static List<LivroModel> livrosVendidos = new();

        private readonly ILivroRepositorio _livroRepositorio;
        public LivroService(ILivroRepositorio livroRepositorio)
        {
            _livroRepositorio = livroRepositorio;
        }
        public bool DevolverLivroPorId(string id)
        {
            var livro = _livroRepositorio.ObterPorId(id);
            if (livro != null && !livro.Disponivel)
            {
                livro.Disponivel = true;
                livro.DevolvidoEm = DateTime.Now.Date;
                _livroRepositorio.Atualizar(livro);
                return true;
            }
            return false;
        }

        public bool EmprestarLivroPorId(string id)
        {
            var livro = _livroRepositorio.ObterPorId(id);
            if (livro != null && livro.Disponivel)
            {
                livro.Disponivel = false;
                livro.EmprestadoEm = DateTime.Now.Date;
                _livroRepositorio.Atualizar(livro);
                return true;
            }
            return false;
        }

        public bool VenderLivro(string id)
        {
            var livro = _livroRepositorio.ObterPorId(id);
            if (livro != null && livro.Disponivel)
            {
                livro.Disponivel = false;
                livro.Vendido = true;
                var listaLivros = _livroRepositorio.Listar();
                if (listaLivros != null)
                {
                    listaLivros.Remove(livro);
                    livrosVendidos.Add(livro);
                }
                return true;
            }
            return false;
        }

        bool ILivro.EmPromocao(string id)
        {
            var livro = _livroRepositorio.ObterPorId(id);
            if (livro != null)
            {
                livro.Promocao = true;
                _livroRepositorio.Atualizar(livro);
                return true;
            }
            return false;
        }

        List<LivroModel> ILivro.ListarHistoricoVendidos()
        {
            var listaVendidos = _livroRepositorio.Listar().Where(l => l.Vendido).ToList();
            livrosVendidos.AddRange(listaVendidos);
            return livrosVendidos;
        }

        List<LivroModel> ILivro.ListarLivrosEmPromocao()
        {
            var listaPromocao = _livroRepositorio.Listar().Where(l => l.Promocao).ToList();
            return listaPromocao;
        }

        List<LivroModel> ILivro.ListarLivrosNovos()
        {
            var listaNovos = _livroRepositorio.Listar().Where(l => l.Novo).ToList();
            return listaNovos;
        }

        List<LivroModel> ILivro.ListarLivrosUsados()
        {
            var listaUsados = _livroRepositorio.Listar().Where(l => !l.Novo).ToList();
            return listaUsados;
        }
    }
}
