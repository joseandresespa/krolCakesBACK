using System.Data;
using MySql.Data.MySqlClient;

namespace krolCakes.Provider
{
    public class DatabaseProvider
    {
        private readonly string _connectionString;

        public DatabaseProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var dataTable = new DataTable();
                    var command = new MySqlCommand(query, connection);

                    // Agregar los parámetros al comando, si hay
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    var dataAdapter = new MySqlDataAdapter(command);
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
