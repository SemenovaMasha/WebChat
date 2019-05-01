using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalR_Identity.Models;
using SignalR_Identity.ViewModels;

namespace SignalR_Identity.Services
{
    public class UserService
    {
        private readonly SignalrContext _context;
        private readonly UserManager<SignalrUser> _userManager;
        private readonly string AvatarsPath;

        public UserService(SignalrContext context, UserManager<SignalrUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            AvatarsPath = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");
        }

        public async Task<SignalrUser> SaveUser(UserProfileViewModel viewModel)
        {
            SignalrUser user = _userManager.Users.Include(u => u.CropAvatar).FirstOrDefault(u => u.Id == viewModel.Id);
            if (viewModel.Avatar != null)
            {
                using (var stream = new FileStream($"{AvatarsPath}\\crop{viewModel.Id}.png", FileMode.Create))
                {
                    await viewModel.Avatar.CopyToAsync(stream);
                }

                if (user.CropAvatar != null)
                {
                    user.CropAvatar.Path = Path.Combine("avatars", $"\\crop{viewModel.Id}.png");
                }
                else
                {
                    user.CropAvatar = new Image() {Path = Path.Combine("avatars", $"\\crop{viewModel.Id}.png")};

                }
            }
            user.BirthDate = viewModel.BirthDate;
            await _userManager.UpdateAsync(user);
            _context.SaveChanges();

            return user;
        }
    }
}
