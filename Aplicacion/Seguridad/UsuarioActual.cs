using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecutar : IRequest<UsuarioData> {}

        public class Manejador : IRequestHandler<Ejecutar, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly IJwtGenerador jwtGenerador;
            private readonly IUsuarioSesion usuarioSesion;

            public Manejador(UserManager<Usuario> _userManager, IJwtGenerador _jwtGenerador, IUsuarioSesion _usuarioSesion)
            {
                userManager = _userManager;
                jwtGenerador = _jwtGenerador;
                usuarioSesion = _usuarioSesion;
            }

            public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByNameAsync(usuarioSesion.ObtenerUsuarioSesion());
                return new UsuarioData 
                {
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    Username = usuario.UserName,
                    Token = jwtGenerador.CrearToken(usuario),
                    Image = null
                };
            }
        }
    }
}