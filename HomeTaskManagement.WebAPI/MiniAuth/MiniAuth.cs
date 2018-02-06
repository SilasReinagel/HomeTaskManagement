using JwtAuthenticator;
using HomeTaskManagement.App.Common;
using Microsoft.AspNetCore.Http;

namespace HomeTaskManagement.WebAPI.Auth
{
    public sealed class MiniAuth
    {
        private readonly Authenticator _auth;

        public MiniAuth(string secret)
        {
            _auth = new Authenticator(HmacEncryptor.CreateSha256(secret));
        }

        public Response<AuthorizedUserId> Validate(HttpRequest req)
        {
            var jwt = new JwtBearerToken(req);
            var result = _auth.Authenticate(jwt.Token);
            var authResult = result.Item1 == Token.Verified 
                ? ValidationResult.Valid
                : new ValidationResult(result.Item1.ToString());
            return authResult.IsValid
                ? jwt.Payload<AuthorizedUserId>()
                : Response<AuthorizedUserId>.Errored(ResponseStatus.Unauthorized, "Unauthorized");
        }
    }
}
