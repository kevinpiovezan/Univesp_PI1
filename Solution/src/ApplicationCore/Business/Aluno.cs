using System;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business
{
    public class Aluno : Entity
    {
        public string Cep { get; set; }
        public string Nome { get; set; }
        public string Nome_Social { get; set; }
        public string Endereco { get; set; }
        public string Cursos { get; set; }
        public string Eixo { get; set; }
        public string Genero { get; set; }
        public string Raca_Cor_Etnia { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string UF { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Tel_Fixo { get; set; }
        public DateTime Ultima_Atualizacao { get; set; }
        public DateTime Data_Emissao { get; set; }
        public DateTime Data_Nascimento { get; set; }

        public bool EnsinoMedio_Escola_Publica { get; set; }
        public bool Cursou_Faculdade { get; set; }
        public bool Professor { get; set; }
        public bool Autorizacao_Imagem { get; set; }
        public bool Cadastro_SpTrans { get; set; }
        public bool Servidor_Publico { get; set; }
        
    }
}
