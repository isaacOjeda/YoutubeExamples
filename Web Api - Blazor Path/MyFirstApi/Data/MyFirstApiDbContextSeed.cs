using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyFirstApi.Models;

namespace MyFirstApi.Data
{
    public class MyFirstApiDbContextSeed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public MyFirstApiDbContextSeed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task Seed()
        {
            var admin = await _userManager.FindByNameAsync("admin");

            if (admin is null)
            {
                var adminRole = new Role
                {
                    Name = "admin",
                    Description = "Puede administrar productos"
                };

                var readerRole = new Role
                {
                    Name = "reader",
                    Description = "Puede ver los productos"
                };

                await _roleManager.CreateAsync(adminRole);
                await _roleManager.CreateAsync(readerRole);

                admin = new User
                {
                    UserName = "admin",
                    Email = "MichaelLGarduno@superrito.com",
                    FirstName = "Michael",
                    LastName = "Garduno"
                };

                await _userManager.CreateAsync(admin, "123456");
                await _userManager.AddToRolesAsync(admin, new List<string> { "admin", "reader" });

                var readerUser = new User
                {
                    UserName = "reader",
                    Email = "DevinHSaladino@gustr.com",
                    PhoneNumber = "732-298-3750",
                    FirstName = "Devin",
                    LastName = "Saladino"
                };

                await _userManager.CreateAsync(readerUser, "123456");
                await _userManager.AddToRoleAsync(readerUser, "reader");
            }
        }
    }
}