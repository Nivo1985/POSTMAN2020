using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using People.Api.Data.Entities;

namespace People.Api.Data
{
    public class PeopleContext: DbContext
    {
        public PeopleContext(DbContextOptions<PeopleContext> options): base(options)
        {
            
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Period> Periods { get; set; }
    }
}
