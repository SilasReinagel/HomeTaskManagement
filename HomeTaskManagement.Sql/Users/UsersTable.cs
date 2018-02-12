using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.Sql.Users
{
    public sealed class UsersTable : IEntityStore<UserRecord>
    {
        private readonly SqlDatabase _db;

        public UsersTable(SqlDatabase db)
        {
            _db = db;
        }

        public Maybe<UserRecord> Get(string id)
        {
            var sql = @"SELECT * 
                        FROM HomeTask.Users
                        WHERE Id = @id";

            return _db.QuerySingle<UserSqlRecord>(sql, new { id })
                .Then(x => x.Value.ToRecord());
        }

        public IEnumerable<UserRecord> GetAll()
        {
            var sql = @"SELECT * 
                        FROM HomeTask.Users";

            return _db.Query<UserSqlRecord>(sql)
                .Select(x => x.ToRecord());
        }

        public void Put(string id, UserRecord record)
        {
            var sql = @"UPDATE HomeTask.Users SET 
                            Username = @username,
                            Name = @name,
                            Roles = @roles
                        WHERE Id = @id;

                        IF @@ROWCOUNT = 0
                        BEGIN
                            INSERT INTO HomeTask.Users (Id, Username, Name, Roles)
                            VALUES (@id, @username, @name, @roles)
                        END";

            _db.Execute(sql, new UserSqlRecord(record));
        }

        public void Remove(string id)
        {
            var sql = @"DELETE FROM HomeTask.Users 
                        WHERE Id = @id";

            _db.Execute(sql, new { id });
        }

        private class UserSqlRecord
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public string Roles { get; set; }

            public UserSqlRecord()
            {
            }

            public UserSqlRecord(UserRecord record)
            {
                Id = record.Id;
                Username = record.Username;
                Name = record.Name;
                Roles = string.Join(",", record.Roles.Select(x => x.ToString()));
            }

            public UserRecord ToRecord()
            {
                return new UserRecord
                {
                    Id = Id,
                    Username = Username,
                    Name = Name,
                    Roles = string.IsNullOrWhiteSpace(Roles) 
                        ? new HashSet<UserRoles>() 
                        : Roles.Split(',').Select(x => (UserRoles)Enum.Parse(typeof(UserRoles), x)).ToHashSet()
                };
            }
        }
    }
}
