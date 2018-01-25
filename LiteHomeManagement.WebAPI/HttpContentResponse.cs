using LiteHomeManagement.App.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace LiteHomeManagement.WebAPI
{
    public sealed class HttpResponse : IActionResult
    {
        private static readonly Dictionary<ResponseStatus, HttpStatusCode> _statusCode = new Dictionary<ResponseStatus, HttpStatusCode>
        {
            { ResponseStatus.AStupidDeveloperForgotToSpecify, HttpStatusCode.InternalServerError },
            { ResponseStatus.BadRequest, HttpStatusCode.BadRequest },
            { ResponseStatus.InvalidState, HttpStatusCode.BadRequest },
            { ResponseStatus.DependencyFailure, HttpStatusCode.BadGateway },
            { ResponseStatus.Errored, HttpStatusCode.InternalServerError },
            { ResponseStatus.UnknownEntity, HttpStatusCode.NotFound },
            { ResponseStatus.Succeeded, HttpStatusCode.OK },
        };

        private readonly Response _resp;

        public HttpResponse(Response resp)
        {
            _resp = resp;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)_statusCode[_resp.Status];
            context.HttpContext.Response.Body = new MemoryStream(Json.ToBytes(_resp));
            return Task.CompletedTask;
        }
    }
}
