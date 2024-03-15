using System;
using System.Text.RegularExpressions;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business
{
    public class Ciclo_Renovacao : Entity
    {
        public int Id_Usuario { get; set; }
        public string Nome { get; set; }
        public DateTime Data_Cadastro { get; set; }
        public bool Ativo { get; set; }
        public int Id_Usuario_Alteracao_Clientes { get; set; }
        public DateTime Data_Alteracao_Clientes { get; set; }
        public int Id_Usuario_Alteracao_Checklist { get; set; }
        public DateTime Data_Alteracao_Checklist { get; set; }
        public string Data_String { get
        {
            return Data_Cadastro.ToString("dd/MM/yyyy HH:mm:ss");
        } } 
        public int Ano { get
        {
            Regex rx = new Regex(@"\d+",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(this.Nome);

            if (matches.Count == 0) return 0;
            foreach (Match match in matches)
            {
                if (match.Success && match.Groups.Count > 0)
                {
                    try
                    {
                        return Int32.Parse(match.Groups[0].Value);
                    } catch(Exception e) { }
                }
            }
            return 1;
        } }
    }
}
