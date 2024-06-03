using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EntityPracticeApp.Areas.Identity.Data
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
