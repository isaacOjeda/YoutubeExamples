

using Microsoft.AspNetCore.Identity;

namespace MyFirstApi.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}