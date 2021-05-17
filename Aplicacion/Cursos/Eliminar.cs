using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
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
                var instructoresBD = context.CursoInstructor.Where(x => x.CursoId == request.Id);

                foreach (var instructor in instructoresBD)
                {
                    context.CursoInstructor.Remove(instructor);
                }

                // quitar comentarios
                var comentariosBD = context.Comentario.Where( x => x.CursoId == request.Id);
                foreach (var cmt in comentariosBD)
                {
                    context.Comentario.Remove(cmt);
                }

                // quitar precio
                var precioBD = context.Precio.Where( x => x.CursoId == request.Id).FirstOrDefault();
                if(precioBD != null)
                {
                    context.Precio.Remove(precioBD);
                }

                var curso = await context.Curso.FindAsync(request.Id);

                if(curso == null)
                {
                    //throw new Exception("No se pudo encontrar el curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontró el curso"});
                }

                context.Remove(curso);

                var resultado = await context.SaveChangesAsync();

                if(resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se puede eliminar el curso");

            }
        }

    }
}