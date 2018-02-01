using System;
using System.Linq;
using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.User
{
    public sealed class Users
    {
        private readonly IEntityStore<UserRecord> _userRecords;

        public Users(IEntityStore<UserRecord> userRecords)
        {
            _userRecords = userRecords;
        }

        public bool Exists(string userId)
        {
            return _userRecords.Contains(x => x.Id.Matches(userId));
        }

        public UserRecord Get(string userId)
        {
            return _userRecords.Get(userId);
        }

        public Response Apply(CreateUser req)
        {
            return Record(x => x.Put(req.Id, 
                    new UserRecord
                    {
                        Id = req.Id,
                        Name = req.Name,
                        Username = req.Username,
                        Roles = new DefaultUserRoles().ToHashSet()
                    }));
        }

        public Response AddRolesToUser(AddRoles req)
        {
            return Record(x => _userRecords.Update(req.UserId, u => u.Roles = u.Roles.Union(req.Roles).ToHashSet()));
        }

        public Response RemoveRolesFromUser(RemoveRoles req)
        {
            return Record(x => _userRecords.Update(req.UserId, u => u.Roles = u.Roles.Except(req.Roles).ToHashSet()));
        }

        private Response Record(Action<IEntityStore<UserRecord>> action)
        {
            try
            {
                action(_userRecords);
                return Response.Success;
            }
            catch (Exception e)
            {
                return Response.Errored(ResponseStatus.DependencyFailure, e.Message);
            }
        }
    }
}
