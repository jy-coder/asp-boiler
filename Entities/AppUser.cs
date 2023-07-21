using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {

        public byte[] PasswordSalt { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public List<Photo> Photos { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }


    }
}