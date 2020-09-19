using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace dotimo.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<Watch> Watches { get; set; }
    }
}