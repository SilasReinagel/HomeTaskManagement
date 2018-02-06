using HomeTaskManagement.App.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeTaskManagement.App.Commands
{
    public sealed class AppCommands
    {
        private readonly Dictionary<string, Func<ICommand>> _commands;

        public AppCommands(Dictionary<string, Func<ICommand>> commands)
        {
            _commands = commands;
        }

        public Response Execute(AppActor actor, string name, string json)
        {
            if (!_commands.ContainsKey(name))
                return Response.Errored(ResponseStatus.BadRequest, $"Unknown command '{name}'");
            var command = _commands[name]();

            try
            {
                var request = Json.ToObject(command.RequestType, json);
                return command.Execute(request);
            }
            catch (ArgumentException e)
            {
                return Response.Errored(ResponseStatus.BadRequest, e.Message);
            }
        }
    }
}
