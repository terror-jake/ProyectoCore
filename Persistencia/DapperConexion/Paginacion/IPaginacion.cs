using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Paginacion
{
    public interface IPaginacion
    {
         Task<PaginacionModel> devolverPaginacion(string storedProcedure, int numeroPagina, int cantidadElementos, 
                IDictionary<string, object> parametrosFiltro, string ordenamientoColumna);
    }
}