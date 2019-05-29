using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;

namespace JOS.ApiKeyAuthentication.Web.Tests
{
    public class AuthorizationIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AuthorizationIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Theory]
        [InlineData("only-employees")]
        [InlineData("only-managers")]
        public async Task GivenUnauthorizedCall_WhenGetOnlyEmployees_ThenReturns403Forbidden(string action)
        {
            var httpClient = _factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/user/{action}");
            var apiKey = "FA872702-6396-45DC-89F0-FC1BE900591B"; // Third party api key
            request.Headers.Add("X-Api-Key", apiKey);

            var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            response.Content.Headers.ContentType.ToString().ShouldBe("application/problem+json");
            responseContent.ShouldBe("{\"type\":\"https://httpstatuses.com/403\",\"title\":\"Forbidden\",\"status\":403}"); // Really naive check, can't guarantee the order of the properties, but whatever :)
            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GivenAuthorizedCall_WhenGetOnlyThirdParties_ThenReturns200Ok()
        {
            var httpClient = _factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "api/user/only-third-parties");
            var apiKey = "FA872702-6396-45DC-89F0-FC1BE900591B"; // Third party api key
            request.Headers.Add("X-Api-Key", apiKey);

            var response = await httpClient.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
