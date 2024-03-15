using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Models
{
    public class AdminModel
    {
        public List<Lista> Listas { get; set; }
        public List<Tabela> Tabelas { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public List<string> ObterColunaTabela(string nomeTabela, string nomeColuna)
        {
            Tabela tabela = Tabelas.Where(w => w.Nome == nomeTabela).ToList().First();

            if (tabela == null)
            {
                throw new ArgumentException("Tabela não existe.");
            }

            TabelaDTO tabelaParsed = JsonSerializer.Deserialize<TabelaDTO>(tabela.Conteudo);

            return tabelaParsed.ObterColuna(nomeColuna);
        }
    }
    public class AdicionarUsuarioModel
    {
        public string Email { get; set; }
        public string Nome { get; set; }
    }

}
