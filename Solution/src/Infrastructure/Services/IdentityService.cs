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
            return "admin@univesp.com";
#else
            return GetUser().Identity.Name;
#endif
        }

        public string ObterNome()
        {
#if DEBUG
            return "Admin univesp";
#else
            return GetUser()?.Claims?.Single(u => u.Type == "name").Value;
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
