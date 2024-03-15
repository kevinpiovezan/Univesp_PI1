using System;
using System.Collections.Generic;
using System.Text;

namespace Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs
{
    public class EmailDTO
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FromEmail { get; set; }
        public List<string> ToEmails { get; set; }
        public string ToEmail { get; set; }
        public int Id { get; set; }
    }
}
