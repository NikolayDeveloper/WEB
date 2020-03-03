using System.Collections.Generic;

namespace Iserv.Niis.Authentication.UserIdentity
{
    public interface IUserIdentity
    {
        int UserId { get; }

        string UserXin { get; }

        string CommonName { get; }

        string Email { get; }

        bool IsInRole(string role);

        bool IsInRoles(List<string> roles);

        bool IsInRole(string[] roles);

        bool IsAuthenticated { get; set; }
    }
}