﻿using Microsoft.AspNetCore.Identity;

namespace EntityPracticeApp.Areas.Identity.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
