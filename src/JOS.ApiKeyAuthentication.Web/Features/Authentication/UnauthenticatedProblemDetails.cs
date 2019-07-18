using Microsoft.AspNetCore.Mvc;

namespace JOS.ApiKeyAuthentication.Web.Features.Authentication
{
    public class UnauthenticatedProblemDetails : ProblemDetails
    {
        public UnauthenticatedProblemDetails(string details = null)
        {
            Title = "Unauthenticated";
            Detail = details;
            Status = 401;
            Type = "https://httpstatuses.com/401";
        }
    }
}
