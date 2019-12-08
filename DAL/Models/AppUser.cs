using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
    }
}