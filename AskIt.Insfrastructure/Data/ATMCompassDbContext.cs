using ATMCompass.Core.Entities;
using ATMCompass.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ATMCompass.Insfrastructure.Data
{
    public class ATMCompassDbContext : DbContext
    {
        public ATMCompassDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ATM> ATMs { get; set; }

        public DbSet<Node> Nodes { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Transport> Transports { get; set; }

        public DbSet<Accommodation> Accommodations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SqlValueReturn<bool>>().HasNoKey().ToView(null);
            modelBuilder.Entity<SqlValueReturn<string>>().HasNoKey().ToView(null);
            modelBuilder.Entity<SqlValueReturn<double>>().HasNoKey().ToView(null);
        }
    }
}
