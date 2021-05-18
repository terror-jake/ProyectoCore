using Dapper;
using System.Data;
using System;

namespace Persistencia.DapperConexion.Instructor
{
        public class InstructorRepositorio : IInstructor
        {
            private readonly IFactoryConnection factoryConnection;

            public InstructorRepositorio(IFactoryConnection _factoryConnection)
            {
                factoryConnection = _factoryConnection;
            }

            public async Task<IEnumerable<InstructorModel>> ObtenerLista()
            {
                IEnumerable<InstructorModel> instructorList = null;
                var storedProcedure = "usp_Obtener_Instructores";
                try
                {
                    var connection = factoryConnection.GetConnection();

                    // puede ser sp pero también query
                    instructorList = await connection.QueryAsync<InstructorModel>(storedProcedure, null, commandType: CommandType.StoredProcedure);
                }
                catch (Exception e)
                {
                    throw new Exception("Error en la consulta de datos: ", e);
                }
                finally
                {
                    factoryConnection.CloseConnection();
                }

                return instructorList;
            }
            public Task<InstructorModel> ObtenerPorId(Guid id)
            {
                throw new NotImplementedException();
            }
            public Task<int> Nuevo(InstructorModel parametros)
            {
                throw new NotImplementedException();
            }
            public Task<int> Actualiza(InstructorModel parametros)
            {
                throw new NotImplementedException();
            }
            public Task<int> Eliminar(Guid id)
            {
                throw new NotImplementedException();
            }
        }
}