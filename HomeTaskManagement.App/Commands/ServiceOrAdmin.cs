using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using System.Linq;

namespace HomeTaskManagement.App.Commands
{
    public sealed class ServiceOrAdmin : ICommand
    {
        private readonly ICommand _command;

        public ServiceOrAdmin(ICommand command)
        {
            _command = command;
        }

        public Response Execute(CommandParams commandParams)
        {
            if (!commandParams.Actor.Roles.Contains(UserRoles.Service) && !commandParams.Actor.Roles.Contains(UserRoles.Admin))
                return Response.Errored(ResponseStatus.Unauthorized, "Requires Service or Admin role user");
            return _command.Execute(commandParams);
        }
    }
}
