using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext context;
            private readonly UserManager<Usuario> userManager;
            private readonly IJwtGenerador jwtGenerador;

            public Manejador(CursosOnlineContext _context, UserManager<Usuario> _userManager, IJwtGenerador _jwtGenerador)
            {
                context = _context;
                userManager = _userManager;
                jwtGenerador = _jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var existe = await context.Users.Where(x => x.Email == request.Email).AnyAsync();

                if(existe)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "Ya existe un usuario registrado con este email"});
                }

                var existeUserName = await context.Users.Where(x => x.UserName == request.Username).AnyAsync();

                if(existeUserName)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "El UserName ya existe"});
                }

                var usuario = new Usuario {
                    NombreCompleto = request.Nombre + " " + request.Apellidos,
                    Email = request.Email,
                    UserName = request.Username
                };

                var resultado = await userManager.CreateAsync(usuario, request.Password);

                if(resultado.Succeeded)
                {
                    return new UsuarioData {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = jwtGenerador.CrearToken(usuario),
                        Username = usuario.UserName,
                        Email = usuario.Email
                    };
                }

                throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new {mensaje = "No se pudo insertar al usuario"});
                

            }
        }
    }
}