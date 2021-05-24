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
        public async Task<int> Nuevo(InstructorModel parametros)
        {
            var storedProcedure = "usp_instructor_nuevo";
            var resultado = null;
            try
            {
                var connection = factoryConnection.GetConnection();
                resultado = await connection.ExecuteAsync(storedProcedure, new
                {
                    InstructorId = Guid.NewGuid(),
                    Nombre = parametros.Nombre,
                    Apellidos = parametros.Apellidos,
                    Titulo = parametros.Titulo
                },
                    CommandType: CommandType.StoredProcedure);

                factoryConnection.CloseConnection();
                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo guardar el nuevo instructor", e);
            }
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