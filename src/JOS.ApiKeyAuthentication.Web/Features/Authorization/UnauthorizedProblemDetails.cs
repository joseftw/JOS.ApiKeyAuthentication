using Microsoft.AspNetCore.Mvc;

namespace JOS.ApiKeyAuthentication.Web.Features.Authorization
{
    public class UnauthorizedProblemDetails : ProblemDetails
    {
        public UnauthorizedProblemDetails(string details = null)
        {
            Title = "Forbidden";
            Detail = details;
            Status = 403;
            Type = "https://httpstatuses.com/403";
        }
    }
}
