using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Persistencia.DapperConexion.Instructor
{
    public interface IInstructor
    {
         Task<IEnumerable<InstructorModel>> ObtenerLista();
         Task<InstructorModel> ObtenerPorId(Guid id);
         Task<int> Nuevo(InstructorModel parametros);
         Task<int> Actualiza(InstructorModel parametros);
         Task<int> Eliminar(Guid id);
    }
}