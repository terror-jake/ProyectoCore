using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest 
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }

            // Crear una prop que represente un arreglo de Guid de Instructor
            public List<Guid> ListaInstructor { get; set; }
            public decimal Precio { get; set; }
            public decimal Promocion { get; set; }

        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
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
                // se agrega guid para obtener la clave que se insertará posterior a CursoInstructor
                Guid _cursoId = Guid.NewGuid();
                var curso = new Curso 
                {
                    CursoId = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };

                context.Curso.Add(curso);

                // Llamar a ListaInstructor e insertar los instructores relacionados con el curso
                if(request.ListaInstructor != null)
                {

                    foreach (var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor
                        {
                            CursoId = _cursoId,
                            InstructorId = id
                        };

                        context.CursoInstructor.Add(cursoInstructor);

                    }
                }

                // inserción de precio
                var precioEntidad = new Precio
                {
                    CursoId = _cursoId,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    PrecioId = Guid.NewGuid()
                };

                context.Precio.Add(precioEntidad);

                // valor = 0, error. valor = 1, se realizó una transacción, valor = 2, dos transacciones
                var valor = await context.SaveChangesAsync();

                if(valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el curso");

            }
        }


    }
}