using System;
using System.Collections.Generic;
using System.Linq;
using Syngenta.Casa.RenCad.ApplicationCore.Enums;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business
{
    public class Arquivo : Entity
    {
        public int Id_Tipo_Arquivo { get; set; }
        public int Id_Cliente { get; set; }
        public int Id_Ciclo_Renovacao { get; set; }
        public int Id_Status_Arquivo { get; set; }
        public int Id_Usuario { get; set; }
        public string Nome { get; set; }
        public string GUID { get; set; }
        public string Comentario { get; set; }
        public string Tipo { get; set; }
        public double Size { get; set; }
        public DateTime Data_Upload { get; set; }
        public DateTime? Data_Revisao { get; set; }
        public int? Id_Usuario_Revisor { get; set; }
        public int? Ano { get; set; }
        public bool Obrigatorio { get; set; }
        public bool Excluido { get; set; }

        public string ObterNomeParaDownload(Tipo_Arquivo ta,int anoCiclo = 0)
        {
            int? ano = Ano != 0 ? Ano : anoCiclo - 1;
            var listaAbreviacoes = new List<Tuple<string,string>>
            {
                new Tuple<string, string>("IR",$"DEC ref. {ano}"),
                new Tuple<string, string>("IR com Recibo",$"DEC REC ref. {ano}"),
                new Tuple<string, string>("Recibo",$"REC ref. {ano}"),
                new Tuple<string, string>("DRE",$"DRE ref. {ano}"),
                new Tuple<string, string>("Balanço",$"BP ref. {ano}"),
                new Tuple<string, string>("Balanço e DRE",$"BP DRE ref. {ano}"),
                new Tuple<string, string>("Ultima Alt.Contrato Social",$"ÚLT.ALT.CONT."),
                new Tuple<string, string>("Estatuto Social",$"ESTATUTO"),
                new Tuple<string, string>("Ata da Assembleia Eleita (Última Eleição Diretores)",$"ATA"),
                new Tuple<string, string>("CAR",$"CAR"),
                new Tuple<string, string>("Balancete",$"Balancete ref. {ano}"),
                new Tuple<string, string>("RG",$"RG"),
                new Tuple<string, string>("CPF",$"CPF"),
                new Tuple<string, string>("CNPJ",$"CNPJ"),
                new Tuple<string, string>("Registro do Comerciante",$"Registro do Comerciante"),
                new Tuple<string, string>("Contrato Social",$"Contrato Social"),
                new Tuple<string, string>("Estatuto Social Social",$"Estatuto Social Social"),
                new Tuple<string, string>("",$""),
            };

            if (Tipo != null && ta.Id != 8)
            {
                return listaAbreviacoes.FirstOrDefault(f => f.Item1 == Tipo).Item2;
            }
            else
            {
                return listaAbreviacoes.FirstOrDefault(f => f.Item1 == ta.Descricao).Item2;
            }
        }
    }
}
