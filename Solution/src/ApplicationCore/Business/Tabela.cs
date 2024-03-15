using System;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business
{
    public class Tabela : Entity
    {
        public string Nome { get; set; }
        public string Conteudo { get; set; }
        public int Id_Usuario_Alteracao { get; set; }
        public DateTime Data_Ultima_Alteracao{ get; set; }
    }
}
