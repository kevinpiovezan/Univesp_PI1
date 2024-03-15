using System.Collections.Generic;
using System.Linq;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.Web.Models
{
    public class AlunoViewModel
    {
        public Aluno Aluno { get; set; }
        public bool Fora_Do_Ciclo { get; set; }
        public int Id_Ciclo_Atual { get; set; }
        public string Status { get; set; }
        public List<Aluno> Alunos { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public Usuario UsuarioLogado { get; set; }
    }
    
}
