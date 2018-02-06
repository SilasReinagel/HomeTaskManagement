using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace HomeTaskManagement.WebAPI.Controllers
{
    public static class AspNetExtensions
    {
        public static string GetRawBodyString(this HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body, Encoding.UTF8))
                return reader.ReadToEnd();
        }
    }
}
