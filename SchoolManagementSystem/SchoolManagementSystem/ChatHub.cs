using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SchoolManagementSystem
{
    public class ChatHub : Hub
    {
        public void SendMessage(int senderId, string senderName, int receiverId, string message)
        {
            // Broadcast the message to all connected clients
            Clients.All.broadcastMessage(senderId, senderName, receiverId, message);
        }

        public override Task OnConnected()
        {
            // You could track user connections here
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // You could track user disconnections here
            return base.OnDisconnected(stopCalled);
        }
    }
}