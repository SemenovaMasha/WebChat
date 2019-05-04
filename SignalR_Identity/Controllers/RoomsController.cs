using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR_Identity.Models;
using SignalR_Identity.Services;
using SignalR_Identity.ViewModels;

namespace SignalR_Identity.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly UserManager<SignalrUser> _userManager;
        private readonly MessageService MessageService;
        private readonly RoomService RoomService;
        private readonly IAuthorizationService _authorizationService;

        public RoomsController(UserManager<SignalrUser> userManager, MessageService messageService, RoomService roomService, IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            MessageService = messageService;
            RoomService = roomService;
            _authorizationService = authorizationService;
        }
        
        public IActionResult Index(Guid roomId)
        {
            RoomViewModel viewModel = new RoomViewModel()
            {
                UserChatGroups = RoomService.GetUserChatGroups(HttpContext.User.Identity.Name),
            };
            ChatGroup chatGroup = RoomService.GetChatGroupById(roomId);

            if (chatGroup != null && !RoomService.IsUserInGroup(HttpContext.User.Identity.Name, roomId))
            {
                viewModel.ErrorMessage = "No access";
            }
            else
            {
                viewModel.ChatGroup = chatGroup;
                viewModel.ErrorMessage = chatGroup == null ? "Choose chat room" : "";
            }

            return View(viewModel);
        }
        
        [HttpGet]
        public async Task<ActionResult> GetMessagesView(Guid chatId)
        {
            RoomMessagesViewModel viewModel = new RoomMessagesViewModel();
            ChatGroup chatGroup = RoomService.GetChatGroupById(chatId);

            if (chatGroup!=null&&!RoomService.IsUserInGroup(HttpContext.User.Identity.Name, chatId))
            {
                viewModel.ErrorMessage = "No access";
                return PartialView("RoomMessages", viewModel);
            }
            
            viewModel.ChatGroup = chatGroup;
            viewModel.ErrorMessage = chatGroup == null ? "Choose chat room" : "";

            return PartialView("RoomMessages", viewModel);
        }

        [HttpGet]
        public IActionResult GetRoomInfo(Guid roomId)
        {
            ChatGroup chatGroup = RoomService.GetChatGroupById(roomId);

            RoomInfoViewModel viewModel;
            if (chatGroup!=null)
            {
                var participants = RoomService.GetChatUsers(roomId);
                viewModel = new RoomInfoViewModel()
                {
                    Id = roomId,
                    Name = chatGroup.Name,
                    Members = participants
                        .Select(u => u.UserName).ToList(),
                    Others = RoomService.GetAllUsers()
                        .Where(i => participants
                            .All(e => i.Id != e.Id))
                        .Select(u=>u.UserName)
                        .ToList()
                };
            }
            else
            {
                viewModel = new RoomInfoViewModel()
                {
                    Name = "New room",
                    Members = new List<string>{ HttpContext.User.Identity.Name },
                    Others = RoomService.GetAllUsers()
                        .Where(u=>u.UserName != HttpContext.User.Identity.Name)
                        .Select(u => u.UserName).ToList()
                };
            }

            return Ok(viewModel);
        }

        [HttpDelete]
        public IActionResult RemoveChat(Guid roomId)
        {
            ChatGroup chatGroup = RoomService.GetChatGroupById(roomId);

            if (chatGroup != null)
            {
                var authorizationResult = _authorizationService
                    .AuthorizeAsync(User, chatGroup, "EditRoomPolicy").Result;

                if (authorizationResult.Succeeded)
                {
                    RoomService.removeChatGroup(chatGroup);
                    return Ok();
                }
                else
                {
                    return StatusCode(403);
                    ;
                }
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult SaveChatGroup([FromBody]RoomInfoViewModel roomInfo)
        {
            if (roomInfo.Id == Guid.Empty)
            {
                RoomService.CreateChatGroup(roomInfo.Name, roomInfo.Members, HttpContext.User.Identity.Name);
            }
            else
            {
                var authorizationResult =  _authorizationService
                    .AuthorizeAsync(User, RoomService.GetChatGroupById(roomInfo.Id), "EditRoomPolicy").Result;

                if (authorizationResult.Succeeded)
                {
                    RoomService.UpdateChatGroup(roomInfo.Id, roomInfo.Name, roomInfo.Members);
                    return Ok(new { isNew = roomInfo.Id == Guid.Empty });
                }
                else 
                {
                    return StatusCode(403); ;
                }

            }

            return Ok(new{ isNew = roomInfo.Id == Guid.Empty});
        }
    }
}