using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.EF_Core.DbContext;

using Microsoft.EntityFrameworkCore;
using SharedProj;

public class ProductDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    //public ProductDbContext()
    //{
    //    this.Database.EnsureCreated();
    //}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer(connectionString: @"Server=localhost;Database=Shop;Trusted_Connection=True;TrustServerCertificate=True");//User Id=admin;Password=admin;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
