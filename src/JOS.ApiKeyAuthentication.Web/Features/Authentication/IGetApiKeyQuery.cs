using System.Threading.Tasks;

namespace JOS.ApiKeyAuthentication.Web.Features.Authentication
{
    public interface IGetApiKeyQuery
    {
        Task<ApiKey> Execute(string providedApiKey);
    }
}
