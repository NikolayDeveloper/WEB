using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Iserv.Niis.Services.Dapper
{
    public class SqlDapperConnection
    {
        public static IEnumerable<TEntity> Query<TEntity>(string sqlQuery, string connectionString)
        {
            using (var sqlConnection = GetSqlConnection(connectionString))
            {
                return sqlConnection.Query<TEntity>(sqlQuery);
            }
        }

        public static async Task<TEntity> QueryFirstOrDefault<TEntity>(string sqlQuery, string connectionString)
        {
            using (var sqlConnection = await GetSqlConnectionAsync(connectionString))
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<TEntity>(sqlQuery, commandTimeout: sqlConnection.ConnectionTimeout);
            }
        }

        public static async Task<dynamic> QueryFirstOrDefault(string sqlQuery, string connectionString)
        {
            using (var sqlConnection = await GetSqlConnectionAsync(connectionString))
            {
                return await sqlConnection.QueryFirstOrDefaultAsync(sqlQuery, commandTimeout: sqlConnection.ConnectionTimeout);
            }
        }

        public static async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sqlQuery, string connectionString)
        {
            using (var sqlConnection = await GetSqlConnectionAsync(connectionString))
            {
                return await sqlConnection.QueryAsync<TEntity>(sqlQuery, commandTimeout: sqlConnection.ConnectionTimeout);
            }
        }

        public static IEnumerable<dynamic> Query(string sqlQuery, string connectionString)
        {
            using (var sqlConnection = GetSqlConnection(connectionString))
            {
                return sqlConnection.Query(sqlQuery);
            }
        }

        public static async Task<IEnumerable<dynamic>> QueryAsync(string sqlQuery, string connectionString)
        {
            using (var sqlConnection = GetSqlConnection(connectionString))
            {
                return await sqlConnection.QueryAsync(sqlQuery);
            }
        }

        public static async Task<int> ExecuteAsync(string sqlQuery, string connectionString)
        {
            using (var sqlConnection = await GetSqlConnectionAsync(connectionString))
            {
                return await sqlConnection.ExecuteAsync(sqlQuery, commandTimeout: sqlConnection.ConnectionTimeout);
            }
        }

        public static async Task<TEntity> ExecuteScalarAsync<TEntity>(string sqlQuery, string connectionString)
        {
            using (var sqlConnection = await GetSqlConnectionAsync(connectionString))
            {
                return await sqlConnection.ExecuteScalarAsync<TEntity>(sqlQuery, commandTimeout: sqlConnection.ConnectionTimeout);
            }
        }

        private static SqlConnection GetSqlConnection(string connectionString)
        {
            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            return sqlConnection;
        }

        private static async Task<SqlConnection> GetSqlConnectionAsync(string connectionString)
        {
            var sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync();

            return sqlConnection;
        }
    }
}