using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task;
using System.Collections.Generic;

namespace HomeTaskManagement.Sql.Tasks
{
    public sealed class TasksTable : IEntityStore<TaskRecord>
    {
        private readonly SqlDatabase _db;

        public TasksTable(SqlDatabase db)
        {
            _db = db;
        }

        public TaskRecord Get(string id)
        {
            var sql = @"SELECT * 
                        FROM HomeTask.Tasks
                        WHERE Id = @id";

            return _db.QuerySingle<TaskRecord>(sql, new { id });
        }

        public IEnumerable<TaskRecord> GetAll()
        {
            var sql = @"SELECT * 
                        FROM HomeTask.Tasks";

            return _db.Query<TaskRecord>(sql);
        }

        public void Put(string id, TaskRecord record)
        {
            var sql = @"UPDATE HomeTask.Tasks SET 
                            Name = @name,
                            UnitsOfWork = @unitsOfWork,
                            Frequency = @frequency,
                            Importance = @importance
                        WHERE Id = @id;

                        IF @@ROWCOUNT = 0
                        BEGIN
                            INSERT INTO HomeTask.Tasks (Id, Name, UnitsOfWork, Frequency, Importance)
                            VALUES (@id, @name, @unitsOfWork, @frequency, @importance)
                        END";

            _db.Execute(sql, 
                new
                {
                    name = record.Name,
                    id = record.Id,
                    unitsOfWork = record.UnitsOfWork,
                    frequency = record.Frequency.ToString(),
                    importance = record.Importance.ToString()
                });
        }

        public void Remove(string id)
        {
            var sql = @"DELETE FROM HomeTask.Tasks 
                        WHERE Id = @id";

            _db.Execute(sql, new { id });
        }
    }
}
