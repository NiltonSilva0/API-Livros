namespace WebAPI1.Models.Cliente_Livro
{
    public class ClienteLivroModel
    {
        public string ClienteId { get; set; } = string.Empty;
        public string LivroId { get; set; } = string.Empty;
        public DateTime? EmprestadoEm { get; set; } = null;
        public DateTime? DevolvidoEm { get; set; } = null;  
        public DateTime? VendidoEm { get; set; } = null;
        public bool Devolvido { get; set; } = false;
        public bool Vendido { get; set; } = false;
    }
}
