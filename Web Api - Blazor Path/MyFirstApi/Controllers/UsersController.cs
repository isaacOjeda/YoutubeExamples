using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Dto.Users;
using MyFirstApi.Helpers;
using MyFirstApi.Models;

namespace MyFirstApi.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public Task<List<UserDto>> GetUsers() =>
            _userManager.Users.Select(s => new UserDto
            {
                Id = s.Id,
                UserName = s.UserName,
                FirstName = s.FirstName,
                LastName = s.LastName
            })
            .ToListAsync();

        [HttpGet("me")]
        public IActionResult Me() =>
            Ok(new 
            {
                Id = User.GetUserId(),
                Email = User.GetUserEmail(),
                User.Identity.Name                
            });
    }
}