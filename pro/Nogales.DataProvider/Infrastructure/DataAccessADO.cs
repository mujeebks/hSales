using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Nogales.DataProvider.Infrastructure
{
    /// <summary>
    /// Data access to fetch data from database using ADO.NET 
    /// </summary>
    public class DataAccessADO : IDisposable
    {

        private readonly string _connectionString;
        public DataAccessADO()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["TsqlConnectionString"].ConnectionString;

        }

        /// <summary>
        /// Execute the sql query and get the result as DataSet
        /// </summary>
        /// <param name="sqlQuery"> sql query to execute</param>
        /// <returns> Result of the query as DataSet </returns>
        public DataSet ReadToDataSet(string sqlQuery)
        {
            var result = new DataSet();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {

                    var sqlCommand = new SqlCommand(sqlQuery, connection);
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 100000000;
                    var adapter = new SqlDataAdapter(sqlCommand);
                    adapter.Fill(result);
                    sqlCommand.Dispose();
                    adapter.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    
                }

                //var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TsqlConnectionString"].ConnectionString);
                //var sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                //sqlCommand.CommandType = CommandType.Text;
                //sqlCommand.CommandTimeout = 100000000;
                //var adapter = new SqlDataAdapter(sqlCommand);
                //adapter.Fill(result);
                //sqlCommand.Dispose();
                //adapter.Dispose();
                //if (sqlConnection.State == ConnectionState.Open)
                //    sqlConnection.Close();
            }
            catch(Exception e)
            {
                // TO DO: Handle exceptions globally
                throw e;
            }
            return result;
        }

        public DataSet ReadToDataSetViaProcedure(string spName, IDbDataParameter[] parameters = null)
        {
            var result = new DataSet();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sqlCommand = new SqlCommand(spName, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 100000000;
                    if (parameters != null)
                    {
                        sqlCommand.Parameters.AddRange(parameters);
                    }

                    var adapter = new SqlDataAdapter(sqlCommand);
                    adapter.Fill(result);
                    sqlCommand.Dispose();
                    adapter.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public int ExecuteQuery(string query)
        {
            var result = 0;
         
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var sqlCommand = new SqlCommand(query, connection);
                    result = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                  //connection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public int ExecuteNonQueryFromStoredProcedure(string spName, IDbDataParameter[] parameters = null)
        {
            var result = 0;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                   

                    connection.Open();
                    var sqlCommand = new SqlCommand(spName, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 100000000;
                    if (parameters != null)
                    {
                        sqlCommand.Parameters.AddRange(parameters);
                    }
                 
                    result = sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public object ExecuteScalar(string query)
        {

           object result = 0;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var sqlCommand = new SqlCommand(query, connection);
                    sqlCommand.CommandTimeout = 0;
                    result = (object)sqlCommand.ExecuteScalar();
                    sqlCommand.Dispose();
                    if (connection.State == ConnectionState.Open)

                        connection.Close();
                    
                        return result;
                }
            }
            catch (Exception ex)
            {
                return result;
            }
        }
        public object ExecuteScalarFromStoredProcedure(string spName, IDbDataParameter[] parameters = null)
        {

            object result = 0;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var sqlCommand = new SqlCommand(spName, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 0;
                    if (parameters != null)
                    {
                        sqlCommand.Parameters.AddRange(parameters);
                    }
                    result = (object)sqlCommand.ExecuteScalar();
                    sqlCommand.Dispose();
                    if (connection.State == ConnectionState.Open)

                        connection.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                return result;
            }
        }
        public void Dispose()
        {
            // Dispose objects
        }
    }
}
