﻿using Microsoft.EntityFrameworkCore;
using Projekt_MVC.Models;

namespace Projekt_MVC.Data
{
    public class ForumDB : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Dyskusja> Dyskusja { get; set; }
        public DbSet<Role> Role { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Dyskusje)
                .WithOne(d => d.Owner)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<User>()
                .HasMany(u => u.PolubioneDyskusje)
                .WithMany(d => d.PolubiajacyUzytkownicy)
                .UsingEntity(j => j.ToTable("UlubioneDyskusje"));

            base.OnModelCreating(modelBuilder);
        }
    }
}