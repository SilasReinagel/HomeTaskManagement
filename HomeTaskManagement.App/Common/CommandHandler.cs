using System;
using System.Collections.Generic;
using System.Text;

namespace HomeTaskManagement.App.Common
{
    public sealed class CommandHandler
    {

        public Response Execute(Command command)
        {
            return Response.Success;
        }
    }
}
