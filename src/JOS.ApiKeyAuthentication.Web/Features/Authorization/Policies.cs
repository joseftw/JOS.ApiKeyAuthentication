namespace JOS.ApiKeyAuthentication.Web.Features.Authorization
{
    public static class Policies
    {
        public const string OnlyEmployees = nameof(OnlyEmployees);
        public const string OnlyManagers = nameof(OnlyManagers);
        public const string OnlyThirdParties = nameof(OnlyThirdParties);
    }
}
