using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Contestor.BlazorServer.Hubs
{
    //[Authorize]
    public class ChatHub : Hub
    {
        //[Authorize]
        public async Task Enter(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Notify", $" вошел в чат в группу {groupName}");
            if (Context.UserIdentifier is string userName)
                await Clients.Group(groupName).SendAsync("Notify", $"{userName} вошел в чат в группу {groupName}");

        }

        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
