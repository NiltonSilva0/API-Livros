using WebAPI1.Dtos.Cliente;
using WebAPI1.Models.Cliente;

namespace WebAPI1.Interfaces.Cliente
{
    public interface IClienteRepositorio
    {
        Task<List<ClienteModel>> AdicionarAsync(List<CriarOuAtualizaClienteDto> cliente);
        List<ClienteModel> Listar();
        ClienteModel ObterPorId(string id);
        ClienteModel Atualizar(ClienteModel cliente);
        ClienteModel Remover(string id);
        List<ClienteModel> ListarInativos();
        Dictionary<string, List<ClienteModel>> ListarPorGenero();
    }
}
