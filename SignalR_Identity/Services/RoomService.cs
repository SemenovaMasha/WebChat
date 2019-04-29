using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalR_Identity.Models;

namespace SignalR_Identity.Services
{
    public class RoomService
    {
        private readonly SignalrContext _context;
        private readonly UserManager<SignalrUser> _userManager;

        public RoomService(SignalrContext context, UserManager<SignalrUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public List<ChatGroup> GetUserChatGroups(string userName)
        {
            SignalrUser user = _context.Users
                .Include(c => c.UserGroups)
                .ThenInclude(ug => ug.ChatGroup)
                .FirstOrDefault(u => u.UserName == userName);
            return user.UserGroups.Select(g => g.ChatGroup).ToList();
        }

        public ChatGroup GetChatGroupById(Guid roomId)
        {
            return _context.ChatGroups
                .Include(g=>g.Messages)
                .ThenInclude(m=>m.User)
                .FirstOrDefault(g => g.Id == roomId);
        }

        public List<SignalrUser> GetChatUsers(Guid roomId)
        {
            ChatGroup chatGroup = _context.ChatGroups
                .Include(c => c.UserGroups)
                .ThenInclude(ug => ug.SignalrUser)
                .FirstOrDefault(u => u.Id == roomId);
            return chatGroup.UserGroups.Select(c => c.SignalrUser).ToList();
        }

        public List<SignalrUser> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public ChatGroup CreateChatGroup(string groupName, List<string> userNames)
        {
            ChatGroup chatGroup = new ChatGroup()
            {
                Name = groupName
            };
            _context.ChatGroups.Add(chatGroup);
            Save();

            foreach (string userName in userNames)
            {
                SignalrUser user = _userManager.FindByNameAsync(userName).Result;

                UserGroup userGroup = new UserGroup()
                {
                    ChatGroup = chatGroup,
                    SignalrUser = user
                };
                _context.UserGroups.Add(userGroup);
            }
            Save();

            return chatGroup;
        }

        public bool IsUserInGroup(SignalrUser user, ChatGroup group)
        {
            SignalrUser userWithGroups = _context.Users.Include(u => u.UserGroups).FirstOrDefault(u => u.Id == user.Id);
            return user.UserGroups.Select(g => g.ChatGroup).Contains(group);
        }

        public bool IsUserInGroup(string userName, Guid groupId)
        {
            SignalrUser userWithGroups = _context.Users.Include(u => u.UserGroups).FirstOrDefault(u => u.UserName == userName);
            ChatGroup chatGroup = _context.ChatGroups.Include(u => u.UserGroups).FirstOrDefault(u => u.Id == groupId);
            return userWithGroups.UserGroups.Select(g => g.ChatGroup).Contains(chatGroup);
        }

        private void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateChatGroup(Guid roomId, string name, List<string> members)
        {
            ChatGroup chatGroup = _context.ChatGroups.Include(c=>c.UserGroups).FirstOrDefault(c => c.Id == roomId);

            foreach (UserGroup userGroup in chatGroup.UserGroups)
            {
                _context.UserGroups.Remove(userGroup);
            }

            chatGroup.Name = name;

            foreach (string userName in members)
            {
                SignalrUser user = _userManager.FindByNameAsync(userName).Result;

                UserGroup userGroup = new UserGroup()
                {
                    ChatGroup = chatGroup,
                    SignalrUser = user
                };
                _context.UserGroups.Add(userGroup);
            }
            Save();
        }

        public void removeChatGroup(ChatGroup chatGroup)
        {
            _context.ChatGroups.Remove(chatGroup);
            Save();
        }
    }
}
