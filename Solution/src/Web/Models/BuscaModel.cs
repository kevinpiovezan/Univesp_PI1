using System.Collections.Generic;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.DTOs;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Models
{
    public class DtParameters
    {
        public int Draw { get; set; }
        public DtColumn[] Columns { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<DtOrder> Order { get; set; }
        public DtSearch Search { get; set; }
        public SearchFiltersDTO Filters { get; set; }
    }

    public class DtColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DtSearch Search { get; set; }
    }
    public class DtSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class FiltersBusca
    {
        
    }
}
