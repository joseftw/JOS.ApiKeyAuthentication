using System.Collections.Generic;
using System.Threading.Tasks;

namespace JOS.ApiKeyAuthentication.Web.Features.Authentication
{
    public interface IGetAllApiKeysQuery
    {
        Task<IReadOnlyDictionary<string, ApiKey>> ExecuteAsync();
    }
}
