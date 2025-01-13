using System;
using System.Data;
using System.Data.SqlClient;

namespace OncoAnalyzer.Services
{
    public class DbExecutor : IDbExecutor
    {
        private readonly string connectionString;
        public DbExecutor(string connectionString)
        {
            // Ensure the connection string is not null or empty
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is not Initialised");
            }
            this.connectionString = connectionString;
        }

        public int ExecuteNonQuery(string query, Action<IDbCommand> parameterize)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    parameterize?.Invoke(command);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public IDataReader ExecuteReader(string query, Action<IDbCommand> parameterize)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                parameterize?.Invoke(command);
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
    }
}
