using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Commands
{
    public interface ICommand
    {
        Response Execute(CommandParams commandParams);
    }
}
