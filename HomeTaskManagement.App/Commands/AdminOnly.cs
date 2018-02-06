using System.Linq;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;

namespace HomeTaskManagement.App.Commands
{
    public sealed class AdminOnly : ICommand
    {
        private readonly ICommand _command;
        
        public AdminOnly(ICommand command)
        {
            _command = command;
        }

        public Response Execute(CommandParams commandParams)
        {
            if (!commandParams.Actor.Roles.Contains(UserRoles.Admin))
                return Response.Errored(ResponseStatus.Unauthorized, "Requires Admin permission");
            return _command.Execute(commandParams);
        }
    }
}
