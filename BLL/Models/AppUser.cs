using IdentityServer4;
using Microsoft.AspNetCore.Identity;

namespace BLL.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
    }
}