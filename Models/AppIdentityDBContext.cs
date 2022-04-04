using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Models
{
    public class AppIdentityDBContext : IdentityDbContext<IdentityUser>
    {

            public AppIdentityDBContext(DbContextOptions options) : base(options)
            {

            }
        
    }
}
