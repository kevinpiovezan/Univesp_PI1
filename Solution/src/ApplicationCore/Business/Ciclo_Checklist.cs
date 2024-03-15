using System;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business
{
    public class Ciclo_Checklist : Entity
    {
        public int Id_Ciclo { get; set; }
        public int Id_Tipo_Arquivo { get; set; }
        public int Id_Tipo_Cliente { get; set; }
        public string Valor { get; set; }
    }
}
