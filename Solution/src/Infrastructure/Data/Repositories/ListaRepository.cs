using Newtonsoft.Json.Linq;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Repositories
{
    public class ListaRepository : Repository<Lista>, IListaRepository
    {
        public ListaRepository(AppDbContext context) : base(context) { }

        public List<string> ObterPorNome(string nome)
        {
            string valoresJson = _Context.Listas.Where(w => w.Nome == nome).Select(s => s.Valores).ToList().FirstOrDefault();

            var valoresJsonParsed = JObject.Parse(valoresJson);
            List<string> valores = valoresJsonParsed["valores"].ToObject<string[]>().ToList();

            return valores;
        }

        public Dictionary<string, List<string>> ObterTodosParsed(bool upper=false)
        {
            Dictionary<string, List<string>> listasParsed = new Dictionary<string, List<string>>();

            List<Lista> listas = _Context.Listas.ToList();

            foreach (Lista lista in listas)
            {

                var valoresJsonParsed = JObject.Parse(lista.Valores);
                List<string> valores;
                if (upper)
                {
                    valores = valoresJsonParsed["valores"].ToObject<string[]>().Select(v => v.ToUpper()).ToList();
                }
                else
                {
                    valores = valoresJsonParsed["valores"].ToObject<string[]>().ToList();
                }

                listasParsed.Add(lista.Nome, valores);

            }

            return listasParsed;
        }
    }
}
