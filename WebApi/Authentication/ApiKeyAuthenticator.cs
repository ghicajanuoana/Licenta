using Microsoft.Extensions.Primitives;

namespace WebApi.Authentication
{
    public class ApiKeyAuthenticator : IApiKeyAuthenticator
    {
        public const string AuthHeaderName = "X-AUTH-APIKEY";
        public const string AuthApiKey = "851B92AC-E52A-42FE-B2E8-2BAEE3B9F369"; 

        private readonly IHttpContextAccessor httpContextAccessor;

        public ApiKeyAuthenticator(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated()
        {
            if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue(AuthHeaderName, out StringValues headerValues))
            {
                return string.Equals(AuthApiKey, headerValues.ToString());
            }

            return false;
        }
    }
}
