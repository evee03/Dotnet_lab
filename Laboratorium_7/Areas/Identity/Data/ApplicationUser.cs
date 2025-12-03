using Microsoft.AspNetCore.Identity;

namespace Laboratorium_7.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        public long CustomerId { get; set; }

        public ApplicationUser()
        {
        }
    }
}
