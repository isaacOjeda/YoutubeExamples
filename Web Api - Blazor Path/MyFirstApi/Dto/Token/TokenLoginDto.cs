using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Dto.Token
{
    public class TokenLoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}