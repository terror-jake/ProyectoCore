using System;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Comentarios
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Alumno { get; set; }
            public int Puntaje { get; set; }
            public string Comentario { get; set; }
            public Guid CursoId { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Alumno).NotEmpty();
                RuleFor(x => x.Puntaje).NotEmpty();
                RuleFor(x => x.Comentario).NotEmpty();
                RuleFor(x => x.CursoId).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly CursosOnlineContext context;

            public Manejador(CursosOnlineContext _context)
            {
                this.context = _context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var comentario = new Comentario
                {
                    ComentarioId = Guid.NewGuid(),
                    Alumno = request.Alumno,
                    ComentarioText = request.Comentario,
                    CursoId = request.CursoId,
                    Puntaje = request.Puntaje
                };

                context.Comentario.Add(comentario);
                var resultados = await context.SaveChangesAsync();

                if(resultados > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo crear el comentario");

            }
        }
    }
}