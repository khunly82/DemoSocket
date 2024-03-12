using DemoSocket.DTO;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace DemoSocket.Hubs
{
    public class ChatHub: Hub
    {
        private static List<NewMessageDTO> _messages = new List<NewMessageDTO>();

        public void CreateMessage(MessageFormDTO message)
        {
            NewMessageDTO m = new NewMessageDTO(
                message.User,
                message.Content,
                DateTime.Now
            );
            _messages.Add(m);
            Clients.All.SendAsync(
                "newMessage", m
            );
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("allMessages", _messages);
            return base.OnConnectedAsync();
        }
    }
}
