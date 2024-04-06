using System;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using System.Collections.Generic;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs
{
    public class SearchFiltersDTO
    {
        public string Nome { get; set; }
        public string Eixo { get; set; }
        public string RA { get; set; }
        public string Cpf { get; set; }
        public string Nascimento { get; set; }
        public int Status_Matricula { get; set; }
        
        public bool? EnsinoMedio_Escola_Publica { get; set; }
        public bool? Cursou_Faculdade { get; set; }
        public bool? Professor { get; set; }
        public bool? Autorizacao_Imagem { get; set; }
        public bool? Cadastro_SpTrans { get; set; }
        public bool? Servidor_Publico { get; set; }
        

    }
}
