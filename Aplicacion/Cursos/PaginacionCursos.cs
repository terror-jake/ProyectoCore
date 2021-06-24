using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Paginacion;

namespace Aplicacion.Cursos
{
    public class PaginacionCursos
    {
        public class Ejecuta : IRequest<PaginacionModel>
        {
            // Filtrar curso por título
            public string Titulo { get; set; }
            public int NumeroPagina { get; set; }
            public int CantidadElementos { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, PaginacionModel>
        {
            private readonly IPaginacion paginacion;
            public Manejador(IPaginacion _paginacion)
            {
                this.paginacion = _paginacion;
            }
            public async Task<PaginacionModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var storedProcedure = "usp_obtener_curso_paginacion";
                var ordenamiento = "Titulo";
                var parametros = new Dictionary<string, object>();
                parametros.Add("NombreCurso", request.Titulo);

                return await paginacion.devolverPaginacion(storedProcedure, request.NumeroPagina, 
                    request.CantidadElementos, parametros, ordenamiento);

                
            }
        }
    }
}