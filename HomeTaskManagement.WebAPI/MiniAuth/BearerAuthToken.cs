using HomeTaskManagement.App.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text;

namespace HomeTaskManagement.WebAPI.Auth
{
    public sealed class JwtBearerToken
    {
        private readonly string _raw;

        public JwtBearerToken(HttpRequest req)
            : this (req.Headers["Authorization"]) { }

        public JwtBearerToken(string raw)
        {
            _raw = raw;
        }

        public string Token => _raw.OrDefault().Replace("Bearer", "").TrimStart(' ');

        public T Payload<T>() => Json.ToObject<T>(GetRawJson());

        private string GetRawJson()
        {
            return Token.Split('.')[1]
                .Then(WebUtility.UrlDecode)
                .Then(Base64Decode)
                .Then(Encoding.UTF8.GetString);
        }

        private byte[] Base64Decode(string s)
        {
            return Convert.FromBase64String(RPad(s, "=", 4));
        }

        private string RPad(string input, string pad, int divisor)
        {
            return input.Length % divisor == 0
                ? input
                : RPad(input, $"{input}{pad}", divisor);
        }
    }
}
