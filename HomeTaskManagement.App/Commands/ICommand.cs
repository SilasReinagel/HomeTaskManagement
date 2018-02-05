using HomeTaskManagement.App.Common;
using System;

namespace HomeTaskManagement.App.Commands
{
    public interface ICommand
    {
        Type RequestType { get; }
        Response Execute(object req);
    }
}
