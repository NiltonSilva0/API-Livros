using WebAPI1.Interfaces.Cliente;
using WebAPI1.Interfaces.Livro;
using WebAPI1.Models.Cliente_Livro;
using WebAPI1.Repositorios.Cliente_Livro;


namespace WebAPI1.Services.Cliente_Livro
{
    public class ClienteLivroService(
        IClienteRepositorio clienteRepositorio,
        ILivroRepositorio livroRepositorio
    ) : IClienteLivroRepositorio
    {
        private readonly List<ClienteLivroModel> bancoClienteLivros = new();
        public List<ClienteLivroModel> ListarPorCliente(string clienteId)
        {
            var cliente = clienteRepositorio.Listar().FirstOrDefault(c => c.Id == clienteId);
            if (cliente == null)
            {
                throw new ArgumentException($"Cliente com ID {clienteId} não encontrado.");
            }
            return bancoClienteLivros.Where(cl => cl.ClienteId == clienteId).ToList();
        }

        public List<ClienteLivroModel> ListarPorLivro(string livroId)
        {
            var livros = bancoClienteLivros.Where(cl => cl.LivroId == livroId).ToList();
            if (livros.Count == 0)
            {
                throw new ArgumentException($"Nenhum registro encontrado para o livro com ID {livroId}.");
            }
            return livros;
        }

        public void RegistrarDevolucao(string clienteId, string livroId)
        {
            var registro = bancoClienteLivros.FirstOrDefault(cl => cl.ClienteId == clienteId && cl.LivroId == livroId && cl.EmprestadoEm != null && !cl.Devolvido);
            if (registro == null)
            {
                throw new ArgumentException($"Nenhum empréstimo ativo encontrado para o cliente {clienteId} e o livro {livroId}.");
            }
            registro.Devolvido = true;
            registro.DevolvidoEm = DateTime.Now.Date;

            // Deixa o livro disponível novamente após devolução
            var livro = livroRepositorio.ObterPorId(livroId);
            if (livro != null)
            {
                livro.Disponivel = true;
                livroRepositorio.Atualizar(livro);
            }
        }

        public void RegistrarEmprestimo(string clienteId, string livroId)
        {
            var cliente = clienteRepositorio.Listar().FirstOrDefault(c => c.Id == clienteId);
            if (cliente == null)
                throw new ArgumentException($"Cliente com ID {clienteId} não encontrado.");

            // Verifica primeiro se já existe empréstimo ativo para este cliente/livro
            var registroExistente = bancoClienteLivros.FirstOrDefault(cl => cl.ClienteId == clienteId && cl.LivroId == livroId && cl.EmprestadoEm != null && !cl.Devolvido);
            if (registroExistente != null)
                throw new ArgumentException($"O cliente {clienteId} já possui um empréstimo ativo para o livro {livroId}.");

            var livro = livroRepositorio.ObterPorId(livroId);
            if (livro == null || !livro.Disponivel)
                throw new ArgumentException($"Livro com ID {livroId} não encontrado ou indisponível.");

            var novoRegistro = new ClienteLivroModel
            {
                ClienteId = clienteId,
                LivroId = livroId,
                EmprestadoEm = DateTime.Now.Date,
                Devolvido = false,
                Vendido = false
            };
            bancoClienteLivros.Add(novoRegistro);

            // Atualiza o status do livro
            livro.Disponivel = false;
            livroRepositorio.Atualizar(livro);
        }

        public void RegistrarVenda(string clienteId, string livroId)
        {
            var cliente = clienteRepositorio.Listar().FirstOrDefault(c => c.Id == clienteId);
            if (cliente == null)
                throw new ArgumentException($"Cliente com ID {clienteId} não encontrado.");

            // Verifica primeiro se o cliente já comprou este livro
            var registroExistente = bancoClienteLivros.FirstOrDefault(cl => cl.ClienteId == clienteId && cl.LivroId == livroId && cl.Vendido);
            if (registroExistente != null)
                throw new ArgumentException($"O cliente {clienteId} já comprou o livro {livroId}.");

            var livro = livroRepositorio.ObterPorId(livroId);
            if (livro == null || !livro.Disponivel)
                throw new ArgumentException($"Livro com ID {livroId} não encontrado ou indisponível.");

            var novoRegistro = new ClienteLivroModel
            {
                ClienteId = clienteId,
                LivroId = livroId,
                Vendido = true,
                EmprestadoEm = null,
                Devolvido = false,
                VendidoEm = DateTime.Now.Date
            };
            bancoClienteLivros.Add(novoRegistro);

            // Atualiza o status do livro
            livro.Disponivel = false;
            livro.Vendido = true;
            livroRepositorio.Atualizar(livro);
        }
    }
}
