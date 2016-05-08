using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace TheWorld.Models
{
    public class WorldContext: IdentityDbContext<WorldUser>
    {

        public WorldContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Trip> Trips { get; set; }

        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            var connString = Startup.configuration["Data:WorldContextConnection"];

            optionBuilder.UseSqlServer(connString);
            
            base.OnConfiguring(optionBuilder);
        }
    }
}
