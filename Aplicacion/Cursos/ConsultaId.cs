using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Persistencia;
using System;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDTO>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<CursoUnico, CursoDTO>
        {
            private readonly CursosOnlineContext context;
            private readonly IMapper mapper;
            public Manejador(CursosOnlineContext _context, IMapper _mapper)
            {
                this.context = _context;
                this.mapper = _mapper;
            }

            public async Task<CursoDTO> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await context.Curso
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructoresLink)
                    .ThenInclude(y => y.Instructor)
                    .FirstOrDefaultAsync( a => a.CursoId == request.Id);

                if(curso == null)
                {
                    //throw new Exception("No se pudo encontrar el curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontró el curso"});
                }

                var cursoDto = mapper.Map<Curso, CursoDTO>(curso);
                
                return cursoDto;
            }
        }
    }
}