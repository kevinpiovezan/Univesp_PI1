using Microsoft.AspNetCore.Mvc;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorWeb.Controllers
{
    [Route("redirect")]
    public class RedirectController : Controller
    {
        public IActionResult Index(string url)
        {
            return View("redirect", url);
        }
    }
}
