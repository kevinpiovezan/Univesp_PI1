using System;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business
{
    public class Ciclo_Cliente : Entity
    {
        public int Id_Ciclo_Renovacao { get; set; }
        public int Id_Cliente { get; set; }
        public int Id_Status_Cliente_Ciclo { get; set; }
        public DateTime? Data_Contatado { get; set; }
        public int? Id_Usuario_Que_Contatou { get; set; }
        public DateTime? Data_Entrega_Documentos { get; set; }
        public bool Cancelado { get; set; }
        public bool Nao_Renovar { get; set; }
        public int? Id_Usuario_Finalizacao { get; set; }
        public DateTime? Data_Finalizacao { get; set; }
        public bool Inclusao { get; set; }
    }
}
