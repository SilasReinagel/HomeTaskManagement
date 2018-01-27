using System;

namespace HomeTaskManagement.App.Common
{
    public sealed class ConsoleLog : ILog
    {
        public void Print(string message)
        {
            Console.WriteLine( message);
        }
    }
}
