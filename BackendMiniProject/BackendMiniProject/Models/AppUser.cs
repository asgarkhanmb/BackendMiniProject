using Microsoft.AspNetCore.Identity;

namespace BackendMiniProject.Models
{
    public class AppUser : IdentityUser
    {
        public  string FullName { get; set; }
    }
}
