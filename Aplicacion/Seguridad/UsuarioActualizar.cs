using System;
using System.Collections.Generic;
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
    public class UsuarioActualizar
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
            private readonly IPasswordHasher<Usuario> passwordHasher;

            public Manejador(CursosOnlineContext _context, UserManager<Usuario> _userManager, IJwtGenerador _jwtGenerador, IPasswordHasher<Usuario> _passwordHasher)
            {
                this.context = _context;
                this.userManager = _userManager;
                this.jwtGenerador = _jwtGenerador;
                this.passwordHasher = _passwordHasher;
            }


            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByNameAsync(request.Username);

                if(usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontró al usuario con el username"} );
                }

                var resultado = await context.Users.Where(x => x.Email == request.Email && x.UserName != request.Username).AnyAsync();

                if(resultado)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new {mensaje = "Este email pertenece a otro usuario"} );
                }

                usuario.NombreCompleto = request.Nombre + " " + request.Apellidos;
                usuario.PasswordHash = passwordHasher.HashPassword(usuario, request.Password);
                usuario.Email = request.Email;

                var resultadoUpdate = await userManager.UpdateAsync(usuario);

                var resultadoRoles = await userManager.GetRolesAsync(usuario);
                var roles = new List<string>(resultadoRoles);

                if(resultadoUpdate.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Username = usuario.UserName,
                        Email = usuario.Email,
                        Token = jwtGenerador.CrearToken(usuario, roles)
                    };
                }

                throw new Exception("No se pudo actualizar el usuario");

            }
        }

    }
}