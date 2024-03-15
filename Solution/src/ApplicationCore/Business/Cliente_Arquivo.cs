using System;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business
{
    public class Cliente_Arquivo : Entity
    {
        public int IdCicloRenovacao { get; set; }
        public int IdCliente { get; set; }
        public int IdArquivo { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Data { get; set; }
        public int? Ano { get; set; }
    }
}
