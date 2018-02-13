using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.App.Common
{
    public static class ResponseExtensions
    {
        public static void AssertStatusIs(this Response response, ResponseStatus status)
        {
            Assert.AreEqual(status, response.Status, response.ErrorMessage);
        }

        public static Response Then<In, Out>(this Response<In> resp, Func<In, Out> map)
        {
            return resp.Succeeded
                ? map(resp.Content)
                : Response.Errored<Out>(resp.Status, resp.ErrorMessage);
        }

        public static Response Then(this Response resp, Func<Response> nextAction)
        {
            return resp.Succeeded
                ? nextAction()
                : resp;
        }

        public static Response And(this Response first, Response second)
        {
            return first.Then(() => second);
        }

        public static Response Combine(this IEnumerable<Response> responses)
        {
            return responses.All(x => x.Succeeded)
                ? Response.Success()
                : Response.Errored(ResponseStatus.Errored, $"Errors: {string.Join(',', responses.Where(x => !x.Succeeded).Select(x => x.ErrorMessage))}");
        }
    }
}
