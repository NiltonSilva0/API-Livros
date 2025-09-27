using WebAPI1.Models.Cliente_Livro;

namespace WebAPI1.Repositorios.Cliente_Livro
{
    public interface IClienteLivroRepositorio
    {
        void RegistrarEmprestimo(string clienteId, string livroId);
        void RegistrarDevolucao(string clienteId, string livroId);
        void RegistrarVenda(string clienteId, string livroId);
        List<ClienteLivroModel> ListarPorCliente(string clienteId);
        List<ClienteLivroModel> ListarPorLivro(string livroId);
    }
}
