using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalR_Identity.Models;

namespace SignalR_Identity.Services
{
    public class MessageService
    {
        private readonly SignalrContext _context;
        private readonly UserManager<SignalrUser> _userManager;
        private readonly RoomService RoomService;

        public MessageService(SignalrContext context, UserManager<SignalrUser> userManager, RoomService roomService)
        {
            _context = context;
            _userManager = userManager;
            RoomService = roomService;
        }

        public Message AddNewMessage(Message message)
        {
            if (RoomService.IsUserInGroup(message.User, message.ChatGroup))
            {
                message.Date = DateTime.Now;
                message = _context.Messages.Add(message).Entity;
                Save();
            }

            return message;
        }

        public Message AddNewMessage(string text, string userName, Guid chatId)
        {
            Message message = new Message()
            {
                Text = text,
                User = _userManager.FindByNameAsync(userName).Result,
                ChatGroup = _context.ChatGroups.FirstOrDefault(g=>g.Id== chatId)
            };
            return AddNewMessage(message);
        }

        public void RemoveOldMessages()
        {
            var r = _context.Messages.Where(m => ( DateTime.Now-m.Date).TotalSeconds >= 20);
            _context.Messages.RemoveRange(_context.Messages
                .Where(m => (m.Date - DateTime.Now).TotalHours >= 24));
                //.Where(m=> (DateTime.Now - m.Date).TotalSeconds>=20));
            Save();
        }

        private void Save()
        {
            _context.SaveChanges();
        }
    }
}
