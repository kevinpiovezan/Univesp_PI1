using Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business;
using System;
using System.Collections.Generic;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.DTOs
{
    public class RelatorioContatoDTO
    {
        public Usuario Usuario { get; set; }
        public Contato Contato { get; set; }
        public Cliente Cliente { get; set; }
        public Ciclo_Renovacao Ciclo{ get; set; }
    }
}
