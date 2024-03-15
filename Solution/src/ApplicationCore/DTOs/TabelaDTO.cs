using System;
using System.Collections.Generic;
using System.Linq;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs
{
    public class TabelaDTO
    {
        public List<string> cabecalho { get; set; }
        public List<LinhaTabelaDTO> linhas { get; set; }

        public List<string> ObterColuna(string coluna, bool valoresDistintos = true, bool toUpper = false)
        {
            int ordemColuna = cabecalho.IndexOf(coluna.ToUpper());

            if (ordemColuna < 0)
            {
                throw new ArgumentException("Coluna '" + coluna + "' não existe na tabela.");
            }

            List<string> valoresColuna = new List<string>();

            if(toUpper)
            {
                valoresColuna = linhas.Select(s => s.colunas[ordemColuna].ToUpper()).ToList();
            } else
            {
                valoresColuna = linhas.Select(s => s.colunas[ordemColuna]).ToList();
            }

            if (valoresDistintos)
            {
                valoresColuna = valoresColuna.Distinct().ToList();
            }

            return valoresColuna;
        }

        public List<string> ObterListaDePara(string colunaDe, string valorDe, string colunaPara, bool isUpper = false)
        {
            int ordemColunaDe = cabecalho.IndexOf(colunaDe.ToUpper());

            if (ordemColunaDe < 0)
            {
                throw new ArgumentException("Coluna '" + colunaDe + "' não existe na tabela.");
            }

            int ordemColunaPara = cabecalho.IndexOf(colunaPara.ToUpper());

            if (ordemColunaPara < 0)
            {
                throw new ArgumentException("Coluna '" + colunaPara + "' não existe na tabela.");
            }

            if(isUpper)
            {
                return linhas.Where(w => w.colunas[ordemColunaDe].ToUpper() == valorDe.ToUpper()).Select(s => s.colunas[ordemColunaPara].ToUpper()).ToList();
            }
            return linhas.Where(w => w.colunas[ordemColunaDe] == valorDe).Select(s => s.colunas[ordemColunaPara]).ToList();

        }

        public List<string> ObterListaDePara(Dictionary<string, string> colunasValoresDe, string colunaPara)
        {
            List<LinhaTabelaDTO> resultado = new List<LinhaTabelaDTO>();
            resultado = linhas;

            foreach (KeyValuePair<string, string> valorColunaDe in colunasValoresDe)
            {
                int ordemColunaDe = cabecalho.IndexOf(valorColunaDe.Key.ToUpper());

                if (ordemColunaDe < 0)
                {
                    throw new ArgumentException("Coluna '" + valorColunaDe.Key + "' não existe na tabela.");
                }

                resultado = resultado.Where(w => w.colunas[ordemColunaDe] == valorColunaDe.Value).ToList();

            }

            int ordemColunaPara = cabecalho.IndexOf(colunaPara.ToUpper());

            if (ordemColunaPara < 0)
            {
                throw new ArgumentException("Coluna '" + colunaPara + "' não existe na tabela.");
            }

            return resultado.Select(s => s.colunas[ordemColunaPara]).ToList();

        }
    }

    public class LinhaTabelaDTO
    {
        public List<string> colunas { get; set; }
    }
}
