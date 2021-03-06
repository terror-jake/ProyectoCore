using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class RolEliminar
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
        }

        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> roleManager;

            public Manejador(RoleManager<IdentityRole> _roleManager)
            {
                this.roleManager = _roleManager;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var rol = await roleManager.FindByNameAsync(request.Nombre);

                if(rol == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "No existe el rol"});
                }

                var resultado = await roleManager.DeleteAsync(rol);

                if(resultado.Succeeded)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el rol");

            }
        }
    }
}