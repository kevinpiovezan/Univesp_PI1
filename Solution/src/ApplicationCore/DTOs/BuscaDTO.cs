using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using System;
using System.Collections.Generic;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs
{
    public class BuscaDTO
    {
        public List<BuscaRes> ResultadosBusca { get; set; }
        public int Qtd_Total { get; set; }
    }
    public class DtOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class BuscaRes
    {
        public Aluno DadosAluno { get; set; }
    }
}
