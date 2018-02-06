﻿using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using System.Linq;

namespace HomeTaskManagement.App.Commands
{
    public sealed class RequiresApprover : ICommand
    {
        private readonly ICommand _command;

        public RequiresApprover(ICommand command)
        {
            _command = command;
        }

        public Response Execute(CommandParams commandParams)
        {
            if (!commandParams.Actor.Roles.Contains(UserRoles.Approver))
                return Response.Errored(ResponseStatus.Unauthorized, "Requires Approver role user");
            return _command.Execute(commandParams);
        }
    }
}
