using WebAPI1.Models;

namespace WebAPI1.Interfaces
{
    public interface ILivroRepositorio
    {
        List<LivroModel> Listar();
        List<LivroModel> ListarPorCategoria(string categoria);
        List<LivroModel> ListarPorAutor(string autor);
        List<LivroModel> ListarPorTitulo(string titulo);
        List<LivroModel> ListarPorAno(int ano);
        List<LivroModel> ListarInativos(string id);
        LivroModel? ObterPorId(string id);
        void Adicionar(List<LivroModel> livro);
        bool Atualizar(LivroModel livro);
        bool Remover(string id);
    }
}
