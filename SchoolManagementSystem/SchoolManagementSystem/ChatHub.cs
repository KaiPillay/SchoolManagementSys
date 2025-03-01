using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace SchoolManagementSystem
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Broadcast the message to all connected clients
            await Clients.All.broadcastMessage(user, message);
        }
    }
}

