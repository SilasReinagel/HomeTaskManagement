using Dapper;
using HomeTaskManagement.App.Common;
using System;
using System.Data;

namespace HomeTaskManagement.Sql
{
    public class UnixUtcTimeTypeHandler : SqlMapper.TypeHandler<UnixUtcTime>
    {
        public override void SetValue(IDbDataParameter parameter, UnixUtcTime value)
        {
            parameter.Value = DateTimeOffset.FromUnixTimeMilliseconds(value).DateTime;
        }

        public override UnixUtcTime Parse(object value)
        {
            return UnixUtcTime.From((DateTime)value);
        }
    }
}
