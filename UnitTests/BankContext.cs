using UnitTests.Bank;
using UnitTests.Customers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BankAccount> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankAccount>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();
        }
    }
}
