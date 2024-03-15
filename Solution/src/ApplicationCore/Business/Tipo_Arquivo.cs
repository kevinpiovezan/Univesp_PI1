using System;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business
{
    public class Tipo_Arquivo : Entity
    {
        public string Descricao { get; set; }
        
        public bool Multiplos { get; set; }
        public bool Fora_De_Ciclo { get; set; }
    }
}
