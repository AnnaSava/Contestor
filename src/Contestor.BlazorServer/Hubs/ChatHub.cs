using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Contestor.BlazorServer.Hubs
{
    public class ChatHub : Hub
    {
        public static Dictionary<string, int> currentUser = new Dictionary<string, int>();

        public async Task Enter(string groupName, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Notify", $"[{DateTime.Now}] {userName} вошел в чат в группу {groupName}");
        }

        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, $"[{DateTime.Now}] {message}");
        }

        public async Task NotifyEveryoneIn()
        {
            var username = Context.User.Claims.Where(s => s.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            if (currentUser.ContainsKey(username))
            {
                currentUser[username]++;
            }
            else
            {
                currentUser.Add(username, 1);
            }

            await Clients.All.SendAsync("Update");
        }

        public async Task NotifyEveryoneOut()
        {
            var username = Context.User.Claims.Where(s => s.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            if (currentUser.Where(s => s.Key == username).First().Value == 1)
            {
                currentUser.Remove(username);
            }
            await Clients.All.SendAsync("Update");
        }

        public async Task Send(string userName, string message)
        {
            var username = Context.User.Claims.Where(s => s.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            await Clients.User(userName).SendAsync("ReceiveMessage", username, message);
        }
    }
}
