using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.Sql.BlobStore
{
    public sealed class SqlBlobStore : IBlobStore
    {
        private readonly SqlDatabase _db;
        private readonly string _tableName;

        public SqlBlobStore(SqlDatabase db, string tableName)
        {
            _db = db;
            _tableName = tableName;
        }

        public Maybe<byte[]> Get(string id)
        {
            var sql = $@"SELECT * 
                        FROM {_tableName} 
                        WHERE Id = @id";

            return _db.QuerySingle<Blob>(sql, new { id })
                .IfPresent(x => x.Value);
        }

        public void Put(string id, byte[] bytes)
        {
            var sql = $@"UPDATE {_tableName} SET
                            Value = @value
                        WHERE Id = @id;

                        IF @@ROWCOUNT = 0
                        BEGIN
                            INSERT INTO {_tableName} (Id, Value)
                            VALUES (@id, @value)
                        END";

            _db.Execute(sql, new { id, value = bytes });
        }

        public void Delete(string id)
        {
            var sql = $@"DELETE FROM {_tableName} WHERE Id = @id";

            _db.Execute(sql, new { id });
        }

        private class Blob
        {
            public string Id { get; set; }
            public byte[] Value { get; set; }
        }
    }
}
