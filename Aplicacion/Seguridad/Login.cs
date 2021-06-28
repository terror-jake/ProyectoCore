using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager; // para encontrar el usuario
            private readonly SignInManager<Usuario> signInManager; // para realizar el acceso

            private readonly IJwtGenerador jwtGenerador;

            // se inyectan dependencias
            public Manejador(UserManager<Usuario> _userManager, SignInManager<Usuario> _signInManager, IJwtGenerador _jwtGenerador)
            {
                userManager = _userManager;
                signInManager = _signInManager;
                jwtGenerador = _jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByEmailAsync(request.Email); // busca por email

                if(usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }

                // false corresponde a si la cuenta debe bloquearse
                var resultado = await signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                var resultadoRoles = await userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(resultadoRoles); // debe ser List
                
                if(resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = jwtGenerador.CrearToken(usuario, listaRoles),
                        Username = usuario.UserName,
                        Email = usuario.Email,
                        Image = null
                    };
                }

                // si falló la autenticación
                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);

            }
        }

    }
}