using Syngenta.Casa.RenCad.AppHub.ApplicationCore.Business;
using System;
using System.Collections.Generic;

namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.DTOs
{
    public class UltimaModificacaoCicloDTO
    {
        public Usuario Usuario_Alteracao_Clientes { get; set; }
        public string Data_Alteracao_Clientes { get; set; }
        public Usuario Usuario_Alteracao_Checklist { get; set; }
        public string Data_Alteracao_Checklist { get; set; }
    }
}
