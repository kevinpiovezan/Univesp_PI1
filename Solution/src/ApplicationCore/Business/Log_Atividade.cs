using System;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business
{
    public class LogAtividade : Entity
    {
        public int Id_Tipo_Atividade { get; set; }
        public int Id_Cliente { get; set; }
        public int Id_Ciclo { get; set; }
        public int Id_Arquivo { get; set; }
        public int Id_Usuario { get; set; }
        public string? Descricao { get; set; }
        public DateTime Data { get; set; }
    }
}
