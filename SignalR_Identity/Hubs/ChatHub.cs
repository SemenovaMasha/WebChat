using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalR_Identity.Models;
using SignalR_Identity.Services;

namespace SignalR_Identity.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MessageService MessageService;
        private readonly RoomService RoomService;

        public ChatHub(MessageService messageService, RoomService roomService)
        {
            MessageService = messageService;
            RoomService = roomService;
        }

        public async Task Send(Guid chatId, string message, string userName)
        {
            if (RoomService.IsUserInGroup(userName, chatId))
            {
                await Clients.Groups(chatId.ToString()).SendAsync("Send", chatId, message, userName);
            }

            MessageService.AddNewMessage(message,userName,chatId);
        }

        public override Task OnConnectedAsync()
        {
            string userName = Context.User.Identity.Name;
            List<ChatGroup> rooms = RoomService.GetUserChatGroups(userName);

            foreach (ChatGroup chatGroup in rooms)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, chatGroup.Id.ToString());
            }

            return base.OnConnectedAsync();
        }
        
    }
}
