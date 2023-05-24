using ATMCompass.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ATMCompass.Insfrastructure.Data
{
    public class ATMCompassDbContext : IdentityDbContext
    {
        public ATMCompassDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ATM> ATMs { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<Location> Locations { get; set; }
    }
}
