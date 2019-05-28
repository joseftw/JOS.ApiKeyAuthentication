using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;

namespace JOS.ApiKeyAuthentication.Web.Tests
{
    public class AuthenticationIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AuthenticationIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task GivenUnauthenticatedCall_WhenGetAnyoneEndpoint_ThenReturns200Ok()
        {
            var httpClient = _factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "api/user/anyone");

            var response = await httpClient.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("only-authenticated")]
        [InlineData("only-employees")]
        [InlineData("only-managers")]
        [InlineData("only-third-parties")]
        public async Task GivenUnauthenticatedCall_WhenGetProtectedEndpoint_ThenReturns401Unauthorized(string action)
        {
            var httpClient = _factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/user/{action}");

            var response = await httpClient.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GivenAuthenticatedCall_WhenGetOnlyAuthenticated_ThenReturns200Ok()
        {
            var httpClient = _factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "api/user/only-authenticated");
            var apiKey = "C5BFF7F0-B4DF-475E-A331-F737424F013C";
            request.Headers.Add("X-Api-Key", apiKey);

            var response = await httpClient.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
