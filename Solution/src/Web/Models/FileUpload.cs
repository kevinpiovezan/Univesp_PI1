using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Models
{
    public class FileUpload
    {
        public IFormFile File { get; set; }
        public string Caminho { get; set; }
    }
}
