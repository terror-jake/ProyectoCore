using Dapper;
using System.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        public async Task<int> Nuevo(string nombre, string apellidos, string titulo)
        {
            var storedProcedure = "usp_instructor_nuevo";
            try
            {
                var connection = factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(storedProcedure, new
                {
                    InstructorId = Guid.NewGuid(),
                    Nombre = nombre,
                    Apellidos = apellidos,
                    Titulo = titulo
                }, commandType: CommandType.StoredProcedure
                ); 

                factoryConnection.CloseConnection();
                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo guardar el nuevo instructor", e);
            }
            finally
            {
                factoryConnection.CloseConnection();
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