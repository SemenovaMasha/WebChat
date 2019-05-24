using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalR_Identity.Models;
using SignalR_Identity.ViewModels;

namespace SignalR_Identity.Services
{
    public class PostService
    {
        private readonly SignalrContext _context;

        public PostService(SignalrContext context)
        {
            _context = context;
        }

        private string GetDefaultAvatarPath() => "/default/FullAvatar.png";

        private string GetUserAvatarPath(SignalrUser user)
        {
            if (user?.CropAvatarId != null)
            {
               return _context.Image.FirstOrDefault(i => i.Id == user.CropAvatarId)?.Path;
            }
            else
            {
                return GetDefaultAvatarPath();
            }
        }

        public List<PostViewModel> GetUsersPosts(string userId, SignalrUser currentUser)
        {
            List <PostViewModel> postViewModels = new List<PostViewModel>();
            foreach (Post post in _context.Posts
                .Where(u=>u.User.Id==userId)
                .Include(p => p.UserLikes)
                .OrderByDescending(p=>p.Date)
                .ToList())
            {
                PostViewModel postViewModel = PostToVM(post);
                if (currentUser != null)
                {
                    postViewModel.UserAuth = true;
                    postViewModel.Liked = post.UserLikes.Select(l => l.SignalrUserId).Contains(currentUser.Id);
                }

                SignalrUser author = _context.Users.FirstOrDefault(u => u.Id == post.AuthorId);
                postViewModel.AuthorAvatarPath = GetUserAvatarPath(author);

                postViewModel.UserId = userId;
                postViewModels.Add(postViewModel);
            }

            return postViewModels;
        }

        private PostViewModel PostToVM(Post post)
        {
            PostViewModel viewModel = new PostViewModel()
            {
                Id = post.Id,
                Date = post.Date,
                AuthorName = _context.Users.FirstOrDefault(u => u.Id == post.AuthorId)?.UserName,
                Text = post.Text
            };
            return viewModel;
        }

        public PostViewModel AddNewPost(PostViewModel viewModel)
        {
            SignalrUser author = _context.Users.FirstOrDefault(u => u.UserName == viewModel.CurrentUsername);
            Post newPost = new Post()
            {
                Date = DateTime.Now,
                Text = viewModel.Text,
                User = _context.Users.FirstOrDefault(u=>u.Id==viewModel.UserId),
                Author = author,
            };
            _context.Posts.Add(newPost);
            _context.SaveChanges();
            viewModel.Date = newPost.Date;
            viewModel.AuthorName = viewModel.CurrentUsername;
            viewModel.AuthorAvatarPath = GetUserAvatarPath(author);
            viewModel.Id = newPost.Id;

            return viewModel;
        }

        public void ChangeLike(Guid postId, string userId)
        {
            UserLike userLike = _context.UserLikes.FirstOrDefault(l => l.PostId == postId && l.SignalrUserId == userId);
            if (userLike == null)
            {
                _context.UserLikes.Add(new UserLike()
                {
                    PostId = postId,
                    SignalrUserId = userId
                });
            }
            else
            {
                _context.UserLikes.Remove(userLike);
            }

            _context.SaveChanges();
        }
    }
}
