using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Paginacion
{
    public class PaginacionRepositorio : IPaginacion
    {
        // LOS TIPOS DE DATO A DEVOLVER DEBEN SER IDICTIONARY
        private readonly IFactoryConnection factoryConnection;

        public PaginacionRepositorio(IFactoryConnection _factoryConnection)
        {
            this.factoryConnection = _factoryConnection;
        }

        public async Task<PaginacionModel> devolverPaginacion(string storedProcedure, int numeroPagina, int cantidadElementos, IDictionary<string, object> parametrosFiltro, string ordenamientoColumna)
        {
            PaginacionModel paginacionModel = new PaginacionModel();

            // rows
            List<IDictionary<string, object>> listaReporte = null;

            // parámetros salida
            int totalRecords = 0;
            int totalPaginas = 0;

            try
            {
                var connection = factoryConnection.GetConnection();

                DynamicParameters parametros = new DynamicParameters();
                
                // agregar dinámicamente parámetros de filtro del cliente
                foreach (var param in parametrosFiltro)
                {
                    parametros.Add("@" + param.Key, param.Value);
                }
                
                
                // Parámetros de entrada
                parametros.Add("@NumeroPagina", numeroPagina);
                parametros.Add("@CantidadElementos", cantidadElementos);
                parametros.Add("@Ordenamiento", ordenamientoColumna);

                // Parámetros de salida
                parametros.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parametros.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);

                var result = await connection.QueryAsync(storedProcedure, parametros, commandType: CommandType.StoredProcedure);

                // hacer query de linq que convierta a IDictionary las rows
                listaReporte = result.Select(x => (IDictionary<string, object>)x ).ToList();

                // agregar parámetros de salida
                paginacionModel.ListaRecords = listaReporte;
                paginacionModel.NumeroPaginas = parametros.Get<int>("@TotalPaginas");
                paginacionModel.TotalRecords = parametros.Get<int>("@TotalRecords");
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo ejecutar el stored procedure", e);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }

            return paginacionModel;
        }
    }
}