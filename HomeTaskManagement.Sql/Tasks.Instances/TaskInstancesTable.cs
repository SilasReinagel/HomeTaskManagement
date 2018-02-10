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

        public Maybe<TaskInstanceRecord> Get(string id)
        {
            var sql = @"SELECT * 
                        FROM HomeTask.TaskInstances
                        WHERE Id = @id";

            return _db.Query<TaskInstanceRecord>(sql, new { id })
                .SingleAsMaybe();
        }

        public IEnumerable<TaskInstanceRecord> GetAll()
        {
            var sql = @"SELECT * 
                        FROM HomeTask.TaskInstances";

            return _db.Query<TaskInstanceRecord>(sql);
        }

        public void Put(string id, TaskInstanceRecord obj)
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
                    id = obj.Id,
                    description = obj.Description,
                    status = obj.Status.ToString(),
                    taskId = obj.TaskId,
                    userId = obj.UserId,
                    due = UnixUtcTime.ToDateTime(obj.Due),
                    price = obj.Price,
                    isFunded = obj.IsFunded,
                    fundedOn = UnixUtcTime.ToDateTime(obj.FundedOn),
                    fundedByUserId = obj.FundedByUserId,
                    updatedStatusAt = UnixUtcTime.ToDateTime(obj.UpdatedStatusAt),
                    updatedStatusByUserId = obj.UpdatedStatusByUserId
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
