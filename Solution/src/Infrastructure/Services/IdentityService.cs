using Microsoft.AspNetCore.Http;
using Univesp.CaminhoDoMar.ProjetoIntegradorApplicationCore.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor accessor;

        public IdentityService(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public string ObterEmail()
        {
#if DEBUG
            return "kevinpiovezan@gmail.com";
#else
            // return GetUser().Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            return  GetUser().Identity.Name;
#endif
        }

        public string ObterNome()
        {
#if DEBUG
            return "Kevin Piovezan";
#else
            return  GetUser().Identity.Name;
#endif
        }

        public bool IsAuthenticated()
        {
#if DEBUG
            return true;
#else
            return GetUser().Identity.IsAuthenticated;
#endif
        }

        public ClaimsPrincipal GetUser() => accessor?.HttpContext?.User;
    }
}
