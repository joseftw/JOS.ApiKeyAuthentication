using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JOS.ApiKeyAuthentication.Web.Features.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IGetAllApiKeysQuery _getAllApiKeysQuery;
        private const string ApiKeyHeaderName = "X-Api-Key";
        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IGetAllApiKeysQuery getAllApiKeysQuery) : base(options, logger, encoder, clock)
        {
            _getAllApiKeysQuery = getAllApiKeysQuery ?? throw new ArgumentNullException(nameof(getAllApiKeysQuery));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            {
                return AuthenticateResult.Fail($"No '{ApiKeyHeaderName}' header was present in the request");
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
            {
                return AuthenticateResult.Fail($"The '{ApiKeyHeaderName}' header value was null or empty");
            }

            var existingApiKeys = await _getAllApiKeysQuery.ExecuteAsync();

            if (existingApiKeys.ContainsKey(providedApiKey))
            {
                var apiKey = existingApiKeys[providedApiKey];

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, apiKey.Owner)
                };

                claims.AddRange(apiKey.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var identity = new ClaimsIdentity(claims, Options.AuthenticationType);

                var identities = new List<ClaimsIdentity> { identity };
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Options.Scheme);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Invalid API Key provided.");
        }
    }
}
