using JwtAuthenticator;
using LiteHomeManagement.App.Common;
using Microsoft.AspNetCore.Http;

namespace LiteHomeManagement.WebAPI.Auth
{
    public class MiniAuth : IValidate<HttpRequest>
    {
        private readonly Authenticator _auth;

        public MiniAuth(string secret)
        {
            _auth = new Authenticator(HmacEncryptor.CreateSha256(secret));
        }

        public ValidationResult Validate(HttpRequest req)
        {
            var result = _auth.Authenticate(new JwtBearerToken(req).Token);
            return result.Item1 == Token.Verified 
                ? ValidationResult.Valid
                : new ValidationResult(result.Item1.ToString());
        }
    }
}
