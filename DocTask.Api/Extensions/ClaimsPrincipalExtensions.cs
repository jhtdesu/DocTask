using System.Security.Claims;

namespace DockTask.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        if (user == null)
        {
            return null;
        }

        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? user.FindFirst("sub")?.Value
               ?? user.FindFirst("nameid")?.Value
               ?? user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value
               ?? user.FindFirst("id")?.Value
               ?? user.Identity?.Name;
    }
}


