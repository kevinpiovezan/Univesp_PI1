namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business
{
    public class Usuario : Entity
    {
        public string Nome { get; set; }
        
        public string Email { get; set; }

        public bool IsAdmin { get; set; }    
    }
}
