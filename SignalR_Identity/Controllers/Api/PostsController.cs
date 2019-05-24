using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR_Identity.Models;
using SignalR_Identity.Services;
using SignalR_Identity.ViewModels;

namespace SignalR_Identity.Controllers.Api
{
    public class PostsController : Controller
    {
        private readonly UserManager<SignalrUser> _userManager;
        private readonly PostService PostService;
        private SignalrUser GetCurrentUser() => _userManager.GetUserAsync(HttpContext.User).Result;

        public PostsController(PostService PostService, UserManager<SignalrUser> userManager)
        {
            this.PostService = PostService;
            _userManager = userManager;
        }
        
        public List<PostViewModel> GetPostsByUserId(string id)
        {
            return PostService.GetUsersPosts(id, GetCurrentUser());
        }
        
        [HttpPost]
        public PostViewModel AddNewPost([FromBody]PostViewModel viewModel)
        {
            return PostService.AddNewPost(viewModel);
        }

        [HttpPost]
        public void ChangeLike(Guid postId)
        {
            PostService.ChangeLike(postId,GetCurrentUser().Id);
        }
    }
}