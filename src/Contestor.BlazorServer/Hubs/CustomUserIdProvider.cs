using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Contestor.BlazorServer.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string? GetUserId(HubConnectionContext connection)
        {
            //var name = connection.User?.Identity?.Name;
            var name = connection.User?.FindFirst(ClaimTypes.Name)?.Value;
            return name;
        }
    }
}
