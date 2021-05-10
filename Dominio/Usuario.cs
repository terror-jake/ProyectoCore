
using Microsoft.AspNetCore.Identity;

namespace Dominio
{
    // Heredar de IdentityUser
    // De no aparecer el using, instalar de NuGet Microsoft.AspNetCore.Identity.EntityFrameworkCore
    public class Usuario : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}