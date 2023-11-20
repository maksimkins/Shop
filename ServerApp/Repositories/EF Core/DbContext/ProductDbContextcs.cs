using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.EF_Core.DbContext;

using Microsoft.EntityFrameworkCore;
using SharedProj;

public class ProductDbContextcs : DbContext
{
    public DbSet<Product> Products { get; set; }

    public ProductDbContextcs()
    {
        this.Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer(connectionString: "Server=localhost;Database=Shop;TrustServerCertificate=True");//User Id=admin;Password=admin;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
