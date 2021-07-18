using Microsoft.AspNetCore.Identity;

namespace MyFirstApi.Models
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }
    }
}