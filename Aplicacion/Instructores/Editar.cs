using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid InstructorId { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Titulo { get; set; }

        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Titulo).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly IInstructor instructorRepositorio;

            public Manejador(IInstructor _instructorRepositorio)
            {
                this.instructorRepositorio = _instructorRepositorio;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                try
                {
                    var resultado = await instructorRepositorio
                        .Actualiza(request.InstructorId, request.Nombre, request.Apellidos, request.Titulo);
                    return Unit.Value;
                }
                catch (Exception e)
                {
                    throw new Exception("No se pudo actualizar al instructor", e);
                }

            }
        }
    }
}