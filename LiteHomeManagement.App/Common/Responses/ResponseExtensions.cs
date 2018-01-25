using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LiteHomeManagement.App.Common
{
    public static class ResponseExtensions
    {
        public static void AssertStatusIs(this Response response, ResponseStatus status)
        {
            Assert.AreEqual(status, response.Status);
        }

        public static Response<Out> Then<In, Out>(this Response<In> resp, Func<In, Out> map)
        {
            return resp.Succeeded
                ? map(resp.Content)
                : Response<Out>.Errored(resp.Status, resp.ErrorMessage);
        }

        public static Response Then(this Response resp, Func<Response> nextAction)
        {
            return resp.Succeeded
                ? nextAction()
                : resp;
        }
    }
}
