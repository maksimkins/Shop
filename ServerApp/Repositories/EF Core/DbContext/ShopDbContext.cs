﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Repositories.EF_Core.DbContext;

using Microsoft.EntityFrameworkCore;
using SharedProj.Models;

public class ShopDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

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
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
