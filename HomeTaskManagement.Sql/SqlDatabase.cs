using Dapper;
using HomeTaskManagement.App.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HomeTaskManagement.Sql
{
    public sealed class SqlDatabase : IMonitoredComponent
    {
        private readonly SqlConnection _conn;

        public string Name => "Sql Connection";

        public SqlDatabase()
            : this(new EnvironmentVariable("HomeTaskManagementSqlConnection")) { }

        public SqlDatabase(string connectionString)
            : this(new SqlConnection(connectionString)) { }

        public SqlDatabase(SqlConnection connection)
        {
            _conn = connection;
        }

        public HealthStatus GetStatus()
        {
            try
            {
                _conn.Execute("SELECT 1");
                return HealthStatus.Healthy;
            }
            catch (Exception)
            {
                return HealthStatus.Unhealthy;
            }
        }

        public T QuerySingle<T>(string sql, object parameters)
        {
            return _conn.Query<T>(sql, parameters).Single();
        }

        public IEnumerable<T> Query<T>(string sql)
        {
            return _conn.Query<T>(sql);
        }

        public void Execute(string sql, object parameters)
        {
            _conn.Execute(sql, parameters);
        }
    }
}
