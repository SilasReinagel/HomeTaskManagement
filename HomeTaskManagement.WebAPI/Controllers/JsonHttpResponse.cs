using HomeTaskManagement.App.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HomeTaskManagement.WebAPI
{
    public sealed class JsonHttpResponse : IActionResult
    {
        private static readonly Dictionary<ResponseStatus, HttpStatusCode> _statusCodes = new Dictionary<ResponseStatus, HttpStatusCode>
        {
            { ResponseStatus.AStupidDeveloperForgotToSpecify, HttpStatusCode.InternalServerError },
            { ResponseStatus.BadRequest, HttpStatusCode.BadRequest },
            { ResponseStatus.InvalidState, HttpStatusCode.BadRequest },
            { ResponseStatus.DependencyFailure, HttpStatusCode.BadGateway },
            { ResponseStatus.Errored, HttpStatusCode.InternalServerError },
            { ResponseStatus.UnknownEntity, HttpStatusCode.NotFound },
            { ResponseStatus.Succeeded, HttpStatusCode.OK },
            { ResponseStatus.Unauthorized, HttpStatusCode.Unauthorized },
        };

        private readonly HttpStatusCode _statusCode;
        private readonly byte[] _content;

        public JsonHttpResponse(object content)
            : this(HttpStatusCode.OK, Json.ToBytes(content)) { }

        public JsonHttpResponse(Response resp)
            : this(_statusCodes[resp.Status], Json.ToBytes(resp)) { }

        public JsonHttpResponse(HttpStatusCode statusCode, byte[] content)
        {
            _statusCode = statusCode;
            _content = content;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)_statusCode;
            context.HttpContext.Response.Body.Write(_content, 0, _content.Length);
            return Task.CompletedTask;
        }
    }
}
