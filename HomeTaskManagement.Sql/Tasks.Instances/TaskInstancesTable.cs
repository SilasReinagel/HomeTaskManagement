using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task.Instance;
using System.Collections.Generic;

namespace HomeTaskManagement.Sql.Tasks.Instances
{
    public sealed class TaskInstancesTable : ITaskInstanceStore
    {
        private readonly SqlDatabase _db;

        public TaskInstancesTable(SqlDatabase db)
        {
            _db = db;
        }

        public TaskInstanceRecord Get(string id)
        {
            var sql = @"SELECT * 
                        FROM HomeTask.TaskInstances
                        WHERE Id = @id";

            return _db.QuerySingle<TaskInstanceRecord>(sql, new { id });
        }

        public IEnumerable<TaskInstanceRecord> GetAll()
        {
            var sql = @"SELECT * 
                        FROM HomeTask.TaskInstances";

            return _db.Query<TaskInstanceRecord>(sql);
        }

        public void Put(string id, TaskInstanceRecord record)
        {
            var sql = @"UPDATE HomeTask.TaskInstances SET 
                            Id = @id,
                            Description = @description,
                            Status = @status,	
                            TaskId = @taskId,
                            UserId = @userId,
                            Due = @due,
                            Price = @price,
	                        IsFunded = @isFunded,
                            FundedOn = @fundedOn,
	                        FundedByUserId = @fundedByUserId,
                            UpdatedStatusAt = @updatedStatusAt,
	                        UpdatedStatusByUserId = @updatedStatusByUserId
                        WHERE Id = @id;

                        IF @@ROWCOUNT = 0
                        BEGIN
                            INSERT INTO HomeTask.TaskInstances (Id, Description, Status, TaskId, UserId, Due, Price, 
                                IsFunded, FundedOn, FundedByUserId, UpdatedStatusAt, UpdatedStatusByUserId)
                            VALUES (@id, @description, @status, @taskId, @userId, @due, @price, 
                                @isFunded, @fundedOn, @fundedByUserId, @updatedStatusAt, @updatedStatusByUserId)
                        END";

            _db.Execute(sql, new
                {
                    id = record.Id,
                    description = record.Description,
                    status = record.Status.ToString(),
                    taskId = record.TaskId,
                    userId = record.UserId,
                    due = UnixUtcTime.ToDateTime(record.Due),
                    price = record.Price,
                    isFunded = record.IsFunded,
                    fundedOn = UnixUtcTime.ToDateTime(record.FundedOn),
                    fundedByUserId = record.FundedByUserId,
                    updatedStatusAt = UnixUtcTime.ToDateTime(record.UpdatedStatusAt),
                    updatedStatusByUserId = record.UpdatedStatusByUserId
                });
        }

        public void Remove(string id)
        {
            var sql = @"DELETE FROM HomeTask.TaskInstances 
                        WHERE Id = @id";

            _db.Execute(sql, new { id });
        }
    }
}
