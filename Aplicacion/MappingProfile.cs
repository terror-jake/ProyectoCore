using System.Linq;
using Aplicacion.Cursos;
using AutoMapper;
using Dominio;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Para InstructoresDTO, mapear de Curso.InstructoresLink que obtiene los datos de select a Instructor
            CreateMap<Curso, CursoDTO>()
                .ForMember(x => x.Instructores, y => y.MapFrom( z => z.InstructoresLink.Select( a => a.Instructor).ToList() ))
                .ForMember(x => x.Comentarios, y => y.MapFrom( z => z.ComentarioLista))
                .ForMember(x => x.Precio, y => y.MapFrom( z => z.PrecioPromocion));

            CreateMap<CursoInstructor, CursoInstructorDTO>();
            CreateMap<Instructor, InstructorDTO>();
            CreateMap<Comentario, ComentarioDTO>();
            CreateMap<Precio, PrecioDTO>();
        }
    }
}