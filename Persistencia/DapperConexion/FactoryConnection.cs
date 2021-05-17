using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;

namespace Persistencia.DapperConexion
{
    public class FactoryConnection : IFactoryConnection
    {

        private IDbConnection connection;
        private readonly IOptions<ConexionConfiguracion> configs;

        public FactoryConnection(IDbConnection _connection)
        {
            connection = _connection;
        }

        public void CloseConnection()
        {
            if(connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public IDbConnection GetConnection()
        {
            if(connection == null)
            {
                connection = new SqlConnection(configs.Value.ConexionSQL);
            }

            if(connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;

        }
    }
}