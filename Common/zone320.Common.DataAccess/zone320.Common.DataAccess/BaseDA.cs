using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using zone320.Common.DataAccess.DTO;
using zone320.Common.DataAccess.SQL;

namespace zone320.Common.DataAccess
{
    public class BaseDA
    {
        private string connectionString;
        public int DefaultTimeout = 30;
        public int RetryCount = 3;
        public int RetryDelayMilliseconds = 100;

        /// <summary>
        /// Database connection
        /// </summary>
        public IDbConnection Connection { get { return new SqlConnection(connectionString); } }

        /// <summary>
        /// Sets connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public void SetConnectionString(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Gets connection string from configuration
        /// </summary>
        /// <param name="configurationKey"></param>
        /// <returns></returns>
        public string GetConnectionStringFromConfiguration(string configurationKey)
        {
            return ConfigurationManager.ConnectionStrings[configurationKey]?.ConnectionString;
        }

        /// <summary>
        /// Queries a stored procedure or SQL and returns a list of results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object parameters = null, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            return (IEnumerable<T>)this.QueryOrExecuteWithRetry<T>(sql, parameters, commandType, commandTimeout, isQuery: true).Result;
        }

        /// <summary>
        /// Executes SQL and returns the number of records affected
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int Execute(string sql, object parameters = null, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            return (int)this.QueryOrExecuteWithRetry<int>(sql, parameters, commandType, commandTimeout, isQuery: false).Result;
        }

        /// <summary>
        /// Queries/executes SQL and retries in the case of transient errors
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="isQuery"></param>
        /// <returns></returns>
        private DataAccessResultDto QueryOrExecuteWithRetry<T>(string sql, object parameters = null, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null, bool isQuery = true)
        {
            var result = new DataAccessResultDto();
            if (isQuery)
            {
                result.Result = new List<T>();
            }
            else
            {
                result.Result = 0;
            }

            if (!string.IsNullOrWhiteSpace(sql))
            {
                this.CheckForBlockedSqlCommands(sql);

                for (int i = 1; i <= this.RetryCount; i++)
                {
                    try
                    {
                        result.NumberOfAttempts++;

                        using (IDbConnection dbConnection = Connection)
                        {
                            dbConnection.Open();
                            if (isQuery)
                            {
                                result.Result = dbConnection.Query<T>(sql, parameters, commandTimeout: this.GetTimeout(commandTimeout), commandType: commandType);
                            }
                            else
                            {
                                result.Result = dbConnection.Execute(sql, parameters, commandTimeout: this.GetTimeout(commandTimeout), commandType: commandType);
                            }
                        }

                        break;
                    }
                    catch (SqlException ex)
                    {
                        if (!this.ShouldRetryError(ex))
                        {
                            throw;
                        }
                        else
                        {
                            Thread.Sleep(this.RetryDelayMilliseconds * i);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Throws an exception if SQL contains blocked SQL commands
        /// </summary>
        /// <param name="sql"></param>
        public void CheckForBlockedSqlCommands(string sql)
        {
            if (!string.IsNullOrWhiteSpace(sql))
            {
                foreach (var command in SqlCommands.BlockedSqlCommands)
                {
                    if (sql.ToUpper().Contains(command))
                    {
                        throw new Exception($"SQL contained blocked command ({command.Trim()}) and was not executed.");
                    }
                }
            }
        }

        /// <summary>
        /// Gets timeout value
        /// </summary>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int GetTimeout(int? commandTimeout = null)
        {
            if (!commandTimeout.HasValue)
            {
                commandTimeout = this.DefaultTimeout;
            }

            return commandTimeout.Value;
        }

        /// <summary>
        /// Checks if a SQL exception should be retried
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool ShouldRetryError(SqlException ex)
        {
            return ex != null && SqlExceptions.TransientSqlExceptions.Contains(ex.Number);
        }

        /// <summary>
        /// Sets primary key if it is empty
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Guid SetPrimaryKeyId(Guid id)
        {
            return this.IsNew(id) ? Guid.NewGuid() : id;
        }

        /// <summary>
        /// Checks if an id is new/empty
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsNew(Guid id)
        {
            return id == Guid.Empty;
        }

        /// <summary>
        /// Compares two objects for equality with optional excluded properties
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="excludedProperties"></param>
        /// <returns></returns>
        public bool HasChanges(object item1, object item2, List<string> excludedProperties = null)
        {
            bool result = false;
            if (item1 != null && item2 != null)
            {
                excludedProperties = this.GetExcludedProperties(excludedProperties);
                foreach (PropertyInfo prop in item1.GetType().GetProperties())
                {
                    if (!excludedProperties.Contains(prop.Name))
                    {
                        var property1 = prop.GetValue(item1, null);
                        var property2 = prop.GetValue(item2, null);

                        result = (property1?.Equals(property2) == false) || (property2?.Equals(property1) == false);
                        if (result)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets excluded properties for comparison
        /// </summary>
        /// <param name="excludedProperties"></param>
        /// <returns></returns>
        public List<string> GetExcludedProperties(List<string> excludedProperties = null)
        {
            var defaultExcludedProperties = typeof(BaseCreateDto).GetProperties().Select(x => x.Name).ToList();
            if (excludedProperties != null)
            {
                defaultExcludedProperties.AddRange(excludedProperties);
            }

            return defaultExcludedProperties;
        }
    }
}
