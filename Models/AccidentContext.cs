using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Models
{
    public class AccidentContext : DbContext
    {
        public AccidentContext()
        {

        }

        public AccidentContext(DbContextOptions<AccidentContext> options)
            : base(options)
        {
        }

        public DbSet<Accident> Accidents { get; set; }

    }
}
