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
            return _userRecords.Get(userId).Value;
        }

        public Response Apply(RegisterUser req)
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

        public Response Apply(UnregisterUser req)
        {
            return Record(x => x.Remove(req.Id));
        }

        public Response Apply(AddRoles req)
        {
            return Record(x => _userRecords.Update(req.UserId, u => u.Roles = u.Roles.Union(req.Roles).ToHashSet()));
        }

        public Response Apply(RemoveRoles req)
        {
            return Record(x => _userRecords.Update(req.UserId, u => u.Roles = u.Roles.Except(req.Roles).ToHashSet()));
        }

        private Response Record(Action<IEntityStore<UserRecord>> action)
        {
            try
            {
                action(_userRecords);
                return Response.Success();
            }
            catch (Exception e)
            {
                return Response.Errored(ResponseStatus.DependencyFailure, e.Message);
            }
        }
    }
}
