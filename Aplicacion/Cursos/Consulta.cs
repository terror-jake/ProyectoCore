using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDTO>> {}
        public class Manejador : IRequestHandler<ListaCursos, List<CursoDTO>>
        {
            private readonly CursosOnlineContext context;
            public Manejador(CursosOnlineContext _context)
            {
                this.context = _context;   
            }

            public async Task<List<CursoDTO>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await context.Curso
                    .Include(x => x.InstructoresLink)
                    .ThenInclude(x => x.Instructor)
                    .ToListAsync();

                return cursos;
            }
        }
    }
}