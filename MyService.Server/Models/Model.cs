using Microsoft.AspNetCore.Identity;

namespace MyService.Server.Models
{
    public class User : IdentityUser
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }
    }
}
