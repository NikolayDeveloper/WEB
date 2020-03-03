using Iserv.Niis.Authentication.UserIdentity;

namespace Iserv.Niis.Authentication
{
    public interface INiisUserAuthenticationService
    {
        IUserIdentity Identity { get; }
    }
}