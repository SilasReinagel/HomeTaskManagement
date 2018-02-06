using HomeTaskManagement.App.Common;
using System;
using System.Collections.Generic;

namespace HomeTaskManagement.App.Commands
{
    public sealed class AppCommands
    {
        private readonly Dictionary<string, ICommand> _commands;

        public AppCommands(Dictionary<string, ICommand> commands)
        {
            _commands = commands;
        }

        public Response Execute(CommandParams commandParams)
        {
            if (!_commands.ContainsKey(commandParams.Name))
                return Response.Errored(ResponseStatus.BadRequest, $"Unknown command '{commandParams.Name}'");

            try
            {
                return _commands[commandParams.Name].Execute(commandParams);
            }
            catch (ArgumentException e)
            {
                return Response.Errored(ResponseStatus.BadRequest, e.Message);
            }
        }
    }
}
