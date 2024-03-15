using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using System.Collections.Generic;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs
{
    public class CargaAlunosDTO
    {
        public string ArquivoBase64 { get; set; }
        public bool Confirmado { get; set; }
    }

    public class CriticasCarga
    {
        public int Linha { get; set; }
        public List<string> Criticas { get; set; }
    }

    public class ResultadoCarga
    {
        public List<CriticasCarga> Criticas_Carga {get; set;}
        public List<Aluno> Alunos { get; set; }
    }
}
