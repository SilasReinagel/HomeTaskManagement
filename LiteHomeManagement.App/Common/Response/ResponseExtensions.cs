using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiteHomeManagement.App.Common
{
    public static class ResponseExtensions
    {
        public static void AssertStatusIs(this Response response, ResponseStatus status)
        {
            Assert.AreEqual(status, response.Status);
        }
    }
}
