using System;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business
{
    public class Contato : Entity
    {
        public int Id_Usuario { get; set; }
        public int Id_Cliente { get; set; }
        public int Id_Ciclo { get; set; }
        public DateTime Data_Registro { get; set; }
        public DateTime Data_Contato { get; set; }
        public DateTime? Data_Entrega { get; set; }
        public string? Comentarios { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
    }
}
