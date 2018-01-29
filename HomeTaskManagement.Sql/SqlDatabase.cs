using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HomeTaskManagement.Sql
{
    public sealed class SqlDatabase
    {
        private readonly SqlConnection _conn;

        public SqlDatabase(string connectionString)
            : this(new SqlConnection(connectionString)) { }

        public SqlDatabase(SqlConnection connection)
        {
            _conn = connection;
        }

        public bool IsHealthy()
        {
            try
            {
                _conn.Execute("SELECT 1");
                return true;
            }
            catch (Exception e)
            {
                return false;
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
