using System;
using System.Collections.Generic;
using System.Text;

namespace LiteHomeManagement.App.Common
{
    public sealed class CommandHandler
    {

        public Response Execute(Command command)
        {
            return Response.Success;
        }
    }
}
