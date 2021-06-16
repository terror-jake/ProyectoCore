using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using AutoMapper;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDTO>> {}
        public class Manejador : IRequestHandler<ListaCursos, List<CursoDTO>>
        {
            private readonly CursosOnlineContext context;
            private readonly IMapper mapper;

            public Manejador(CursosOnlineContext _context, IMapper _mapper)
            {
                this.context = _context;   
                this.mapper = _mapper;   
            }

            public async Task<List<CursoDTO>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await context.Curso
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructoresLink)
                    .ThenInclude(x => x.Instructor)
                    .ToListAsync();

                var cursosDto = mapper.Map<List<Curso>, List<CursoDTO>>(cursos);

                return cursosDto;
            }
        }
    }
}