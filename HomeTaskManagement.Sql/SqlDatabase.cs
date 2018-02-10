using Dapper;
using HomeTaskManagement.App.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HomeTaskManagement.Sql
{
    public sealed class SqlDatabase : IMonitoredComponent
    {
        private readonly ReflectionValidation _validation = new ReflectionValidation();
        private readonly SqlConnection _conn;

        public string Name => "Sql Connection";

        static SqlDatabase()
        {
            SqlMapper.AddTypeHandler(new UnixUtcTimeTypeHandler());
            SqlMapper.AddTypeMap(typeof(UnixUtcTime), DbType.DateTime2);
            SqlMapper.AddTypeMap(typeof(DateTime), DbType.DateTime2);
        }

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

        public void UsingConnection(Action<IDbConnection> action)
        {
            _conn.Open();
            action(_conn);
            _conn.Close();
        }

        public Maybe<T> QuerySingle<T>(string sql, object parameters)
        {
            return _conn.Query<T>(sql, parameters)
                .SingleAsMaybe()
                .Then(x => ThrowExceptionIfNotValid(sql, parameters, x));
        }

        public IEnumerable<T> Query<T>(string sql)
        {
            return _conn.Query<T>(sql)
                .Select(item => ThrowExceptionIfNotValid(sql, new { }, item));
        }

        public IEnumerable<T> Query<T>(string sql, object parameters)
        {
            return _conn.Query<T>(sql, parameters)
                .Select(item => ThrowExceptionIfNotValid(sql, parameters, item));
        }

        public void Execute(string sql, object parameters)
        {
            _conn.Execute(sql, parameters);
        }

        private T ThrowExceptionIfNotValid<T>(string sql, object parameters, T result)
        {
            _validation.Validate(result).IfInvalid(x => throw new ArgumentException(
                $"Loaded record from SQL invalid: '{x.IssuesMessage}'. Query '{sql}'. Parameters {Json.ToString(parameters)}"));
            return result;
        }
    }
}
