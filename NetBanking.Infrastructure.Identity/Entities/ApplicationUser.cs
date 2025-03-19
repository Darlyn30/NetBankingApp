using Microsoft.AspNetCore.Identity;

namespace NetBanking.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string IdentificationNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
