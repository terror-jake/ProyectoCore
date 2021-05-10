using System.Linq;
using System.Security.Claims;
using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;

namespace Seguridad.Token
{
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UsuarioSesion(IHttpContextAccessor _httpContextAccesor)
        {
            httpContextAccessor = _httpContextAccesor;
        }
        public string ObtenerUsuarioSesion()
        {
            var username = httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return username;
        }
    }
}