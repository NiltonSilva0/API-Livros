using WebAPI1.Dtos.Cliente;
using WebAPI1.Interfaces.Cliente;
using WebAPI1.Models.Cliente;

namespace WebAPI1.Repositorios.Cliente
{
    public class ClienteRepositorio() : IClienteRepositorio
    {
        private readonly List<ClienteModel> bancoClientes = [];
        public async Task<List<ClienteModel>> AdicionarAsync(List<CriarOuAtualizaClienteDto> cliente)
        {
            var nowUtc = DateTime.UtcNow;
            var refYear = nowUtc.Year < 1900 ? 1900 : nowUtc.Year;

            var emailsExistentes = new HashSet<string>(bancoClientes.Count > 0 ? bancoClientes.Select(c => c.Email) : Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);

            var clientes = new List<ClienteModel>(cliente.Count);
            foreach (var item in cliente)
            {
                if ((uint)(item.DataNascimento.Year - 1900) > (uint)(refYear - 1900))
                {
                    throw new ArgumentException($"Data de nascimento inválida para o cliente {item.Nome}: {item.DataNascimento}");
                }
                if (string.IsNullOrEmpty(item.Nome) || string.IsNullOrEmpty(item.Email) || string.IsNullOrEmpty(item.Telefone) || string.IsNullOrEmpty(item.Genero))
                {
                    throw new ArgumentException($"Dados incompletos para o cliente: {item.Nome}");
                }
                if (emailsExistentes.Contains(item.Email))
                {
                    throw new ArgumentException($"Email já cadastrado: {item.Email}");
                }
                emailsExistentes.Add(item.Email);

                clientes.Add(new ClienteModel
                {
                    Id = string.IsNullOrWhiteSpace(item.Id) ? Guid.NewGuid().ToString() : item.Id,
                    Nome = item.Nome,
                    Email = item.Email,
                    Telefone = item.Telefone,
                    Genero = item.Genero,
                    DataNascimento = item.DataNascimento,
                    DataCadastro = nowUtc,
                    Ativo = item.Ativo
                });
            }

            bancoClientes.AddRange(clientes);

            await Task.CompletedTask;

            return bancoClientes;
        }

        public ClienteModel Atualizar(ClienteModel cliente)
        {
            if (cliente == null)
                return new ClienteModel();

            var clienteExistente = bancoClientes.FirstOrDefault(c => c.Id == cliente.Id);
            if (clienteExistente == null)
                return new ClienteModel();

            clienteExistente.Nome = cliente.Nome;
            clienteExistente.Email = cliente.Email;
            clienteExistente.Telefone = cliente.Telefone;
            clienteExistente.Genero = cliente.Genero;
            clienteExistente.DataNascimento = cliente.DataNascimento;
            return clienteExistente;
        }

        public List<ClienteModel> Listar()
        {
            if (bancoClientes.Count > 0)
            {
                return bancoClientes.Where(c => c.Ativo).ToList();
            }
            return new List<ClienteModel>();
        }

        public List<ClienteModel> ListarInativos()
        {
            if (bancoClientes.Count > 0)
            {
                return bancoClientes.Where(c => !c.Ativo).ToList();
            }
            return new List<ClienteModel>();
        }

        public Dictionary<string, List<ClienteModel>> ListarPorGenero()
        {
            if (bancoClientes.Count > 0)
            {
                return bancoClientes
                    .GroupBy(c => c.Genero)
                    .ToDictionary(g => g.Key, g => g.ToList());
            }
            return new Dictionary<string, List<ClienteModel>>();
        }

        public ClienteModel ObterPorId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new ClienteModel();
            var cliente = bancoClientes.FirstOrDefault(c => c.Id == id);
            return cliente ?? new ClienteModel();
        }

        public ClienteModel Remover(string id)
        {
            var clienteRemover = bancoClientes.FirstOrDefault(c => c.Id == id);
            if (clienteRemover != null)
            {
                clienteRemover.Ativo = false;
                return clienteRemover;
               }
               return new ClienteModel();
           }
    }
}
