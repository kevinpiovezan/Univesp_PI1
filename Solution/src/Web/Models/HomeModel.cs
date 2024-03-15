using System;
using System.Collections.Generic;
using System.Linq;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.Web.Models
{
    public class HomeModel
    {
        public Usuario UsuarioLogado { get; set; }
        public List<Usuario> TodosUsuarios { get; set; }
        public List<Aluno> Alunos { get; set; }

    }
    public class DadosDashboard
    {
        public List<Aluno> Alunos { get; set; }
        public DadosDashboard(List<Aluno> alunos)
        {
            Alunos = alunos;
        }

        public class GraphData
        {
            public List<String> Labels
            {
                get; set;
            }
            public List<int> Values
            {
                get; set;
            }
            
        }
    }

}
