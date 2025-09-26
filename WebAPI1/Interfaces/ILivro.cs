using WebAPI1.Models;

namespace WebAPI1.Interfaces
{
    public interface ILivro
    {
        bool EmprestarLivroPorId(string id);
        bool DevolverLivroPorId(string id);
        bool VenderLivro(string id);
        bool EmPromocao(string id);
        List<LivroModel> ListarLivrosEmPromocao();
        List<LivroModel> ListarLivrosNovos();
        List<LivroModel> ListarLivrosUsados();
        List<LivroModel> ListarHistoricoVendidos();
    }
}
