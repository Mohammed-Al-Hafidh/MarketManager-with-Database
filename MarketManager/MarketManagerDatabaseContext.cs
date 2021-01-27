using System;
using System.IO;
using System.Data.Entity;

namespace MarketManager
{
    class MarketManagerDatabaseContext : DbContext
    {
        const string databasename = "MarketManagerDatabase.mdf";
        static string DbPath = Path.Combine(Environment.CurrentDirectory, databasename);
        public MarketManagerDatabaseContext() : base($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DbPath};Integrated Security=True;Connect Timeout=30")        
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Inventory> Inventories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Product>()
                        .HasRequired(p => p.Inventory)
                        .WithRequiredPrincipal(i => i.Product);
        }

    }
}
