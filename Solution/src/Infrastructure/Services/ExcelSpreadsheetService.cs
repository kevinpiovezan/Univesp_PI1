using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Enums;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Services
{
    public static class ExcelSpreadsheetService
    {
        private static string formato_data_hora = "dd/MM/yyyy hh:mm";
        private static Regex rx_cpf = new Regex(@"^(((\d{3}).(\d{3}).(\d{3})-\d{2}))$",
          RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static Object _lock = new object();
        public static string ExportarJSON(Stream arquivo, List<string> cabecalhoValidacao)
        {

            // Abrindo arquivo Excel

            ExcelPackage excel = new ExcelPackage(arquivo);

            ExcelWorksheet planilha = excel.Workbook.Worksheets[0];

            // Tabela que será exportada para JSON

            TabelaDTO tabela = new TabelaDTO();

            // Exportando cabeçalho

            tabela.cabecalho = new List<string>();

            for (int i = 1; i <= planilha.Dimension.End.Column; i++)
            {
                if (planilha.Cells[1, i].Value == null)
                {
                    throw new ArgumentException("Cabeçalho do arquivo inválido.");
                }
                if (!String.IsNullOrEmpty(planilha.Cells[1, i].Value.ToString()))
                {
                    tabela.cabecalho.Add(planilha.Cells[1, i].Value.ToString().ToUpper());
                }
                else
                {
                    break;
                }
            }

            int qtdColunas = tabela.cabecalho.Count;

            // Validando cabeçalho

            for (int i = 0; i < tabela.cabecalho.Count; i++)
            {
                try
                {
                    if (tabela.cabecalho[i].ToUpper() != cabecalhoValidacao[i].ToUpper())
                    {
                        throw new ArgumentException("Cabeçalho do arquivo inválido.");
                    }

                }
                catch
                {
                    throw;
                }
            }

            // Exportando linhas

            tabela.linhas = new List<LinhaTabelaDTO>();

            for (int r = 2; r <= planilha.Dimension.End.Row; r++)
            {
                LinhaTabelaDTO linha = new LinhaTabelaDTO();
                linha.colunas = new List<string>();

                for (int c = 1; c <= qtdColunas; c++)
                {
                    if (planilha.Cells[r, c].Value != null && !String.IsNullOrEmpty(planilha.Cells[r, c].Value.ToString()))
                    {
                        linha.colunas.Add(planilha.Cells[r, c].Text.ToString());
                    }
                    else
                    {
                        linha.colunas.Add("");
                    }
                }

                tabela.linhas.Add(linha);

            }

            // Retornando json

            return JsonSerializer.Serialize(tabela);
        }

        private static int AdicionarHeader(ExcelWorksheet worksheet, Dictionary<string, int> indice, string header, int width = 0)
        {
            int column = indice.Count() + 1;

            worksheet.Column(column).Width = width != 0 ? width : header.Length * 2;
            worksheet.Row(1).Height = 25;
            worksheet.Cells[1, column].Value = header;
            worksheet.Cells[1, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[1, column].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(175, 24, 23));
            worksheet.Cells[1, column].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[1, column].Style.Font.Bold = true;
            worksheet.Cells[1, column].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            indice[header] = column;
            return column;
        }

        public static MemoryStream ExportarTabela(String nomeTabela, TabelaDTO tabela)
        {
            // Criando arquivo do Excel

            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add(nomeTabela);

            // Criando header e alimentando o indice

            Dictionary<string, int> indiceHeader = new Dictionary<string, int>();


            foreach (String campo in tabela.cabecalho)
            {
                AdicionarHeader(workSheet, indiceHeader, campo);
            }

            int currentRow = 2;
            foreach (LinhaTabelaDTO linha in tabela.linhas)
            {
                int currentCell = 1;
                foreach (String campo in linha.colunas)
                {
                    workSheet.Cells[currentRow, currentCell].Value = campo;
                    currentCell++;
                }
                currentRow++;
            }

            // Retornando memory stream

            MemoryStream excelStream = new MemoryStream();

            excel.SaveAs(excelStream);

            excelStream.Position = 0;

            return excelStream;
        }


        public static MemoryStream ExportarAlunos(List<Aluno> alunos)
        {
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Clientes");
            Dictionary<string, int> indiceHeader = new Dictionary<string, int>();

            List<string> cabecalho = new List<string>() { "Status Matricula","Nome","RA","Nome Social", "Data de Nascimento", "CPF", "RG", "UF", "Data de Emissão", "E-mail pessoal", "Endereco", "CEP", "Celular",
                "Telefone Fixo", "Gênero", "Raça/Cor/Etnia","Cursou Ensino Médio em Escola Pública?", "Já cursou alguma faculdade?", "Se sim, quais cursos?", "É professor?",
                "É Servidor Público?", "Eixo escolhido na UniCeu", "Cadastro Bilhete de estudante(SPTrans)","Autorização de Imagem" };
            foreach (string c in cabecalho)
            {
                AdicionarHeader(workSheet, indiceHeader, c);
            }


            int currentRow = 2;
            
            foreach (var a in alunos)
            {
                string statusMatricula = string.Empty;

                switch (a.Id_Status_Matricula)
                {
                    case 1:
                        statusMatricula = EStatus_Matricula.ATIVA.ToString();
                        break;
                    case 2:
                        statusMatricula = EStatus_Matricula.TRANCADA.ToString();
                        break;
                    case 3:
                        statusMatricula = EStatus_Matricula.CONCLUIDA.ToString();
                        break;
                    case 4:
                        statusMatricula = EStatus_Matricula.CANCELADA.ToString();
                        break;
                }
                workSheet.Cells[currentRow, 1].Value = statusMatricula;
                workSheet.Cells[currentRow, 2].Value = a.Nome;
                workSheet.Cells[currentRow, 3].Value = a.RA;
                workSheet.Cells[currentRow, 4].Value = a.Nome_Social;
                workSheet.Cells[currentRow, 5].Value = a.Data_Nascimento.ToString("dd/MM/yyyy");
                workSheet.Cells[currentRow, 6].Value = a.Cpf;
                workSheet.Cells[currentRow, 7].Value = a.Rg;
                workSheet.Cells[currentRow, 8].Value = a.UF;
                workSheet.Cells[currentRow, 9].Value = a.Data_Emissao.ToString("dd/MM/yyyy");
                workSheet.Cells[currentRow, 10].Value = a.Email;
                workSheet.Cells[currentRow, 11].Value = a.Endereco;
                workSheet.Cells[currentRow, 12].Value = a.Cep;
                workSheet.Cells[currentRow, 13].Value = a.Celular;
                workSheet.Cells[currentRow, 14].Value = a.Tel_Fixo;
                workSheet.Cells[currentRow, 15].Value = a.Genero;
                workSheet.Cells[currentRow, 16].Value = a.Raca_Cor_Etnia;
                workSheet.Cells[currentRow, 17].Value = a.EnsinoMedio_Escola_Publica ? "Sim" : "Não";
                workSheet.Cells[currentRow, 18].Value = a.Cursou_Faculdade ? "Sim" : "Não";
                workSheet.Cells[currentRow, 19].Value = a.Cursos;
                workSheet.Cells[currentRow, 20].Value = a.Professor ? "Sim" : "Não";
                workSheet.Cells[currentRow, 21].Value = a.Servidor_Publico ? "Sim" : "Não";
                workSheet.Cells[currentRow, 22].Value = a.Eixo;
                workSheet.Cells[currentRow, 23].Value = a.Cadastro_SpTrans ? "Sim" : "Não";
                workSheet.Cells[currentRow, 24].Value = a.Autorizacao_Imagem ? "Sim" : "Não";
                currentRow++;
            }

            // Retornando memory stream

            MemoryStream excelStream = new MemoryStream();

            excel.SaveAs(excelStream);

            excelStream.Position = 0;

            return excelStream;
        }

        public static ResultadoCarga CargaExcelAlunos(Stream arquivo)
        {
            ExcelPackage excel = new ExcelPackage(arquivo);

            ExcelWorksheet planilha = excel.Workbook.Worksheets[0];
            List<string> cabecalho = new List<string>() { "Status Matricula","Nome","RA","Nome Social", "Data de Nascimento", "CPF", "RG", "UF", "Data de Emissão", "E-mail pessoal", "Endereco", "CEP", "Celular",
                "Telefone Fixo", "Gênero", "Raça/Cor/Etnia","Cursou Ensino Médio em Escola Pública?", "Já cursou alguma faculdade?", "Se sim, quais cursos?", "É professor?",
                "É Servidor Público?", "Eixo escolhido na UniCeu", "Cadastro Bilhete de estudante(SPTrans)","Autorização de Imagem" };

            ResultadoCarga res = new ResultadoCarga();
            res.Criticas_Carga = new List<CriticasCarga>();

            for (int i = 1; i <= cabecalho.Count; i++)
            {
                if (planilha.Cells[1, i].Text != cabecalho[i - 1])
                {
                    res.Criticas_Carga.Add(new CriticasCarga()
                    {
                        Linha = 1,
                        Criticas = new List<string>() { "Cabeçalho inválido" }
                    });
                    return res;
                }
            }


            res.Alunos = new List<Aluno>();
            for (int r = 2; r <= planilha.Dimension.End.Row; r++)
            {
                CriticasCarga critica = new CriticasCarga();
                critica.Linha = r;
                critica.Criticas = new List<string>();
                int statusMatricula = 0;
                switch (planilha.Cells[r, 1].Text)
                {
                    case "ATIVA":
                        statusMatricula = 1;
                        break;
                    case "TRANCADA":
                        statusMatricula = 2;
                        break;
                    case "CONCLUIDA":
                        statusMatricula = 3;
                        break;
                    case "CANCELADA":
                        statusMatricula = 4;
                        break;
                }
                Aluno a = new Aluno()
                {
                    Id_Status_Matricula = statusMatricula,
                    Nome = planilha.Cells[r, 2].Text,
                    RA = planilha.Cells[r, 3].Text,
                    Nome_Social = planilha.Cells[r, 4].Text,
                    Data_Nascimento = DateTime.ParseExact(planilha.Cells[r, 5].Text,"dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Cpf = planilha.Cells[r, 6].Text,
                    Rg = planilha.Cells[r, 7].Text,
                    UF = planilha.Cells[r, 8].Text,
                    Data_Emissao = DateTime.ParseExact(planilha.Cells[r, 9].Text,"dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Email = planilha.Cells[r, 10].Text,
                    Endereco = planilha.Cells[r, 11].Text,
                    Cep = planilha.Cells[r, 12].Text,
                    Celular = planilha.Cells[r, 13].Text,
                    Tel_Fixo = planilha.Cells[r, 14].Text,
                    Genero = planilha.Cells[r, 15].Text,
                    Raca_Cor_Etnia = planilha.Cells[r, 16].Text,
                    EnsinoMedio_Escola_Publica = planilha.Cells[r, 17].Text.ToLower() == "sim",
                    Cursou_Faculdade = planilha.Cells[r, 18].Text.ToLower() == "sim",
                    Cursos = planilha.Cells[r, 19].Text,
                    Professor = planilha.Cells[r, 20].Text.ToLower() == "sim",
                    Servidor_Publico = planilha.Cells[r, 21].Text.ToLower() == "sim",
                    Eixo = planilha.Cells[r, 22].Text,
                    Cadastro_SpTrans = planilha.Cells[r, 23].Text.ToLower() == "sim",
                    Autorizacao_Imagem = planilha.Cells[r, 24].Text.ToLower() == "sim",
                    Ultima_Atualizacao = DateTime.Now
                };

                if (a.Id_Status_Matricula == 0)
                {
                    critica.Criticas.Add("O Status da matricula adicionado não existe");
                }
                if (a.Nome == "")
                {
                    critica.Criticas.Add("'Nome' não pode ficar vazio");
                }
                if (a.RA == "")
                {
                    critica.Criticas.Add("'RA' não pode ficar vazio");
                }
                if (a.Data_Nascimento == DateTime.MinValue)
                {
                    critica.Criticas.Add("A data de nascimento não pode ser vazia");
                }
                if (a.Rg == "")
                {
                    critica.Criticas.Add("O Rg não pode ser vazio");
                }
                if (a.UF == "")
                {
                    critica.Criticas.Add("O campo UF não pode ser vazio");
                }
                if (a.Cpf == "")
                {
                    critica.Criticas.Add("'CPF' não pode ficar vazio");
                }
                if (!rx_cpf.IsMatch(a.Cpf))
                {
                    critica.Criticas.Add("O CPF não está no padrão correto");
                }
                if (a.Email == "")
                {
                    critica.Criticas.Add("'E-mail' não pode ficar vazio");
                }
                if (a.Endereco == "")
                {
                    critica.Criticas.Add("'Endereço' não pode ficar vazio");
                }
                if (a.Cep == "")
                {
                    critica.Criticas.Add("'Cep' não pode ficar vazio");
                }
                if (a.Celular == "")
                {
                    critica.Criticas.Add("'Celular' não pode ficar vazio");
                }
                if (a.Genero == "")
                {
                    critica.Criticas.Add("'Gênero' não pode ficar vazio");
                }                
                if (a.Raca_Cor_Etnia == "")
                {
                    critica.Criticas.Add("'Raça/Cor/Etnia' não pode ficar vazio");
                }
                if (planilha.Cells[r, 17].Text == "")
                {
                    critica.Criticas.Add("'Cursou Ensino Médio em Escola Pública?' não pode ficar vazio");
                }
                if (planilha.Cells[r, 18].Text == "")
                {
                    critica.Criticas.Add("'Já cursou alguma faculdade?' não pode ficar vazio");
                }
                if (planilha.Cells[r, 20].Text == "")
                {
                    critica.Criticas.Add("'É professor?' não pode ficar vazio");
                }
                if (planilha.Cells[r, 21].Text == "")
                {
                    critica.Criticas.Add("'É Servidor Público?' não pode ficar vazio");
                }
                if (planilha.Cells[r, 22].Text == "")
                {
                    critica.Criticas.Add("'Eixo' não pode ficar vazio");
                }
                if (planilha.Cells[r, 23].Text == "")
                {
                    critica.Criticas.Add("'Cadastro Bilhete de estudante(SPTrans)' não pode ficar vazio");
                }
                if (planilha.Cells[r, 24].Text == "")
                {
                    critica.Criticas.Add("'Autorização de Imagem' não pode ficar vazio");
                }

                if (critica.Criticas.Count > 0)
                    res.Criticas_Carga.Add(critica);
                res.Alunos.Add(a);

            }

            var duplicatas = res.Alunos.GroupBy(x => x.Cpf)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            if (duplicatas.Count > 0)
            {
                var criticas = new List<string>();

                foreach (string cpf in duplicatas)
                {
                    criticas.Add("CPF " + cpf + "  duplicado no excel");
                }

                res.Criticas_Carga.Add(new CriticasCarga()
                {
                    Criticas = criticas
                });
            }

            return res;
        }

        public async static Task<MemoryStream> ExportarRelatorio(List<Aluno> alunos)
        {
            MemoryStream excelStream = new MemoryStream();

            await Task.Run(async () =>
            {

                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Alunos");
                Dictionary<string, int> indiceHeader = new Dictionary<string, int>();

                List<string> cabecalho = new List<string>()
                {
                    "Última atualização", "Status Matricula","Nome","RA","Nome Social", "Data de Nascimento", "CPF", "RG", "UF", "Data de Emissão", "E-mail pessoal", "Endereco", "CEP", "Celular",
                    "Telefone Fixo", "Gênero", "Raça/Cor/Etnia","Cursou Ensino Médio em Escola Pública?", "Já cursou alguma faculdade?", "Se sim, quais cursos?", "É professor?",
                    "É Servidor Público?", "Eixo escolhido na UniCeu", "Cadastro Bilhete de estudante(SPTrans)","Autorização de Imagem"
                };

                foreach (string c in cabecalho)
                {
                    AdicionarHeader(workSheet, indiceHeader, c);
                }

                int currentRow = 2;

                for (var i = 0; i < alunos.Count; i++)
                {
                    var a = alunos[i];
                    string statusMatricula = string.Empty;

                    switch (a.Id_Status_Matricula)
                    {
                        case 1:
                            statusMatricula = EStatus_Matricula.ATIVA.ToString();
                            break;
                        case 2:
                            statusMatricula = EStatus_Matricula.TRANCADA.ToString();
                            break;
                        case 3:
                            statusMatricula = EStatus_Matricula.CONCLUIDA.ToString();
                            break;
                        case 4:
                            statusMatricula = EStatus_Matricula.CANCELADA.ToString();
                            break;
                    }
                    workSheet.Cells[currentRow, 1].Value = a.Ultima_Atualizacao.ToString("dd/MM/yyyy HH:mm");
                    workSheet.Cells[currentRow, 2].Value = statusMatricula;
                    workSheet.Cells[currentRow, 3].Value = a.Nome;
                    workSheet.Cells[currentRow, 4].Value = a.RA;
                    workSheet.Cells[currentRow, 5].Value = a.Nome_Social;
                    workSheet.Cells[currentRow, 6].Value = a.Data_Nascimento.ToString("dd/MM/yyyy");
                    workSheet.Cells[currentRow, 7].Value = a.Cpf;
                    workSheet.Cells[currentRow, 8].Value = a.Rg;
                    workSheet.Cells[currentRow, 9].Value = a.UF;
                    workSheet.Cells[currentRow, 10].Value = a.Data_Emissao.ToString("dd/MM/yyyy");
                    workSheet.Cells[currentRow, 11].Value = a.Email;
                    workSheet.Cells[currentRow, 12].Value = a.Endereco;
                    workSheet.Cells[currentRow, 13].Value = a.Cep;
                    workSheet.Cells[currentRow, 14].Value = a.Celular;
                    workSheet.Cells[currentRow, 15].Value = a.Tel_Fixo;
                    workSheet.Cells[currentRow, 16].Value = a.Genero;
                    workSheet.Cells[currentRow, 17].Value = a.Raca_Cor_Etnia;
                    workSheet.Cells[currentRow, 18].Value = a.EnsinoMedio_Escola_Publica ? "Sim" : "Não";
                    workSheet.Cells[currentRow, 19].Value = a.Cursou_Faculdade ? "Sim" : "Não";
                    workSheet.Cells[currentRow, 20].Value = a.Cursos;
                    workSheet.Cells[currentRow, 21].Value = a.Professor ? "Sim" : "Não";
                    workSheet.Cells[currentRow, 22].Value = a.Servidor_Publico ? "Sim" : "Não";
                    workSheet.Cells[currentRow, 23].Value = a.Eixo;
                    workSheet.Cells[currentRow, 24].Value = a.Cadastro_SpTrans ? "Sim" : "Não";
                    workSheet.Cells[currentRow, 25].Value = a.Autorizacao_Imagem ? "Sim" : "Não";

                    currentRow++;
                }

                excel.SaveAs(excelStream);

                excelStream.Position = 0;

                return excelStream;
            }, new CancellationToken());
            return excelStream;
        }

    }
}
namespace System
{
    public static class StringExt
    {
        public static bool ContainsInsensitive(this string source, string search)
        {
            return (new CultureInfo("pt-BR").CompareInfo).IndexOf(source, search, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }
    }
}