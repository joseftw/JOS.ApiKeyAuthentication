using Microsoft.AspNetCore.Mvc;

namespace JOS.ApiKeyAuthentication.Web.Features.Authentication
{
    public class UnauthenticatedProblemDetails : ProblemDetails
    {
        public UnauthenticatedProblemDetails(string details = null)
        {
            Title = "Unauthenticated";
            Detail = details;
            Status = 403;
            Type = "https://httpstatuses.com/403";
        }
    }
}
